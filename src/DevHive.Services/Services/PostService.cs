using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Comment;
using DevHive.Services.Models.Post.Post;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevHive.Services.Interfaces;
using DevHive.Data.Interfaces.Repositories;
using System.Linq;

namespace DevHive.Services.Services
{
	public class PostService : IPostService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper _postMapper;

		public PostService(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, IMapper postMapper)
		{
			this._userRepository = userRepository;
			this._postRepository = postRepository;
			this._commentRepository = commentRepository;
			this._postMapper = postMapper;
		}

		#region Create
		public async Task<Guid> CreatePost(CreatePostServiceModel createPostServiceModel)
		{
			if (!await this._userRepository.DoesUserExistAsync(createPostServiceModel.CreatorId))
				throw new ArgumentException("User does not exist!");

			Post post = this._postMapper.Map<Post>(createPostServiceModel);
			post.TimeCreated = DateTime.Now;

			bool success = await this._postRepository.AddAsync(post);
			if (success)
			{
				Post newPost = await this._postRepository
					.GetPostByCreatorAndTimeCreatedAsync(post.CreatorId, post.TimeCreated);

				return newPost.Id;
			}
			else
				return Guid.Empty;
		}

		public async Task<Guid> AddComment(CreateCommentServiceModel createCommentServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(createCommentServiceModel.PostId))
				throw new ArgumentException("Post does not exist!");

			Comment comment = this._postMapper.Map<Comment>(createCommentServiceModel);
			comment.TimeCreated = DateTime.Now;

			bool success = await this._commentRepository.AddAsync(comment);
			if (success)
			{
				Comment newComment = await this._commentRepository
					.GetCommentByIssuerAndTimeCreatedAsync(comment.CreatorId, comment.TimeCreated);

				return newComment.Id;
			}
			else
				return Guid.Empty;
		}
		#endregion

		#region Read
		public async Task<ReadPostServiceModel> GetPostById(Guid id)
		{
			Post post = await this._postRepository.GetByIdAsync(id) ??
				throw new ArgumentException("The post does not exist!");

			User user = await this._userRepository.GetByIdAsync(post.CreatorId) ??
				throw new ArgumentException("The user does not exist!");

			ReadPostServiceModel readPostServiceModel = this._postMapper.Map<ReadPostServiceModel>(post);
			readPostServiceModel.CreatorFirstName = user.FirstName;
			readPostServiceModel.CreatorLastName = user.LastName;
			readPostServiceModel.CreatorUsername = user.UserName;

			return readPostServiceModel;
		}

		public async Task<ReadCommentServiceModel> GetCommentById(Guid id)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(id) ??
				throw new ArgumentException("The comment does not exist");

			User user = await this._userRepository.GetByIdAsync(comment.CreatorId) ??
				throw new ArgumentException("The user does not exist");

			ReadCommentServiceModel readCommentServiceModel = this._postMapper.Map<ReadCommentServiceModel>(comment);
			readCommentServiceModel.IssuerFirstName = user.FirstName;
			readCommentServiceModel.IssuerLastName = user.LastName;
			readCommentServiceModel.IssuerUsername = user.UserName;

			return readCommentServiceModel;
		}
		#endregion

		#region Update
		public async Task<Guid> UpdatePost(UpdatePostServiceModel updatePostServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(updatePostServiceModel.PostId))
				throw new ArgumentException("Post does not exist!");

			Post post = this._postMapper.Map<Post>(updatePostServiceModel);
			post.TimeCreated = DateTime.Now;

			bool result = await this._postRepository.EditAsync(updatePostServiceModel.PostId, post);

			if (result)
				return (await this._postRepository.GetByIdAsync(updatePostServiceModel.PostId)).Id;
			else
				return Guid.Empty;
		}

		public async Task<Guid> UpdateComment(UpdateCommentServiceModel updateCommentServiceModel)
		{
			if (!await this._commentRepository.DoesCommentExist(updateCommentServiceModel.CommentId))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = this._postMapper.Map<Comment>(updateCommentServiceModel);
			comment.TimeCreated = DateTime.Now;

			bool result = await this._commentRepository.EditAsync(updateCommentServiceModel.CommentId, comment);

			if (result)
				return (await this._commentRepository.GetByIdAsync(updateCommentServiceModel.CommentId)).Id;
			else
				return Guid.Empty;
		}
		#endregion

		#region Delete
		public async Task<bool> DeletePost(Guid id)
		{
			if (!await this._postRepository.DoesPostExist(id))
				throw new ArgumentException("Post does not exist!");

			Post post = await this._postRepository.GetByIdAsync(id);
			return await this._postRepository.DeleteAsync(post);
		}

		public async Task<bool> DeleteComment(Guid id)
		{
			if (!await this._commentRepository.DoesCommentExist(id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = await this._commentRepository.GetByIdAsync(id);
			return await this._commentRepository.DeleteAsync(comment);
		}
		#endregion

		#region Validations
		public async Task<bool> ValidateJwtForPost(Guid postId, string rawTokenData)
		{
			Post post = await this._postRepository.GetByIdAsync(postId) ??
				throw new ArgumentException("Post does not exist!");
			User user = await this.GetUserForValidation(rawTokenData);

			//If user made the post
			if (post.CreatorId == user.Id)
				return true;
			//If user is admin
			else if (user.Roles.Any(x => x.Name == Role.AdminRole))
				return true;
			else
				return false;
		}

		public async Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(commentId) ??
				throw new ArgumentException("Comment does not exist!");
			User user = await this.GetUserForValidation(rawTokenData);

			//If user made the comment
			if (comment.CreatorId == user.Id)
				return true;
			//If user is admin
			else if (user.Roles.Any(x => x.Name == Role.AdminRole))
				return true;
			else
				return false;
		}

		private async Task<User> GetUserForValidation(string rawTokenData)
		{
			JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			Guid jwtUserId = Guid.Parse(this.GetClaimTypeValues("ID", jwt.Claims).First());
			//HashSet<string> jwtRoleNames = this.GetClaimTypeValues("role", jwt.Claims);

			User user = await this._userRepository.GetByIdAsync(jwtUserId) ??
				throw new ArgumentException("User does not exist!");

			return user;
		}

		private List<string> GetClaimTypeValues(string type, IEnumerable<Claim> claims)
		{
			List<string> toReturn = new();

			foreach (var claim in claims)
				if (claim.Type == type)
					toReturn.Add(claim.Value);

			return toReturn;
		}
		#endregion
	}
}
