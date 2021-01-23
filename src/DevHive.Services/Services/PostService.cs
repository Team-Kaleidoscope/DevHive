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
			Post post = this._postMapper.Map<Post>(createPostServiceModel);
			post.TimeCreated = DateTime.Now;

			bool success = await this._postRepository.AddAsync(post);
			if (success)
			{
				Post newPost = await this._postRepository
					.GetPostByCreatorAndTimeCreatedAsync(createPostServiceModel.IssuerId, createPostServiceModel.TimeCreated);

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
			createCommentServiceModel.TimeCreated = DateTime.Now;

			bool success = await this._commentRepository.AddAsync(comment);
			if (success)
			{
				Comment newComment = await this._commentRepository
					.GetCommentByIssuerAndTimeCreatedAsync(createCommentServiceModel.IssuerId, createCommentServiceModel.TimeCreated);

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

			return this._postMapper.Map<ReadPostServiceModel>(post);
		}

		public async Task<ReadCommentServiceModel> GetCommentById(Guid id)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(id) ??
				throw new ArgumentException("The comment does not exist");

			return this._postMapper.Map<ReadCommentServiceModel>(comment);
		}
		#endregion

		#region Update
		public async Task<Guid> UpdatePost(UpdatePostServiceModel updatePostServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(updatePostServiceModel.PostId))
				throw new ArgumentException("Post does not exist!");

			Post post = this._postMapper.Map<Post>(updatePostServiceModel);
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
			Post post = await this._postRepository.GetByIdAsync(postId);
			User user = await this.GetUserForValidation(rawTokenData);

			return post.CreatorId == user.Id;
		}

		public async Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(commentId);
			User user = await this.GetUserForValidation(rawTokenData);

			return comment.IssuerId == user.Id;
		}

		private async Task<User> GetUserForValidation(string rawTokenData)
		{
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			string jwtUserName = this.GetClaimTypeValues("unique_name", jwt.Claims).First();
			//HashSet<string> jwtRoleNames = this.GetClaimTypeValues("role", jwt.Claims);

			User user = await this._userRepository.GetByUsernameAsync(jwtUserName)
				?? throw new ArgumentException("User does not exist!");

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
