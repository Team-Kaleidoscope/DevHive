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
		private readonly IPostRepository _postRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _postMapper;

		public PostService(IPostRepository postRepository, IUserRepository userRepository, IMapper postMapper)
		{
			this._postRepository = postRepository;
			this._userRepository = userRepository;
			this._postMapper = postMapper;
		}

		//Create
		public async Task<Guid> CreatePost(CreatePostServiceModel postServiceModel)
		{
			Post post = this._postMapper.Map<Post>(postServiceModel);

			bool success = await this._postRepository.AddAsync(post);

			if(success)
			{
				Post newPost = await this._postRepository.GetPostByIssuerAndTimeCreatedAsync(postServiceModel.IssuerId, postServiceModel.TimeCreated);
				return newPost.Id;
			}
			else
				return Guid.Empty;
		}

		public async Task<Guid> AddComment(CreateCommentServiceModel commentServiceModel)
		{
			commentServiceModel.TimeCreated = DateTime.Now;
			Comment comment = this._postMapper.Map<Comment>(commentServiceModel);

			bool success = await this._postRepository.AddCommentAsync(comment);

			if(success)
			{
				Comment newComment = await this._postRepository.GetCommentByIssuerAndTimeCreatedAsync(commentServiceModel.IssuerId, commentServiceModel.TimeCreated);
				return newComment.Id;
			}
			else
				return Guid.Empty;
		}

		//Read
		public async Task<PostServiceModel> GetPostById(Guid id)
		{
			Post post = await this._postRepository.GetByIdAsync(id)
				?? throw new ArgumentException("Post does not exist!");

			return this._postMapper.Map<PostServiceModel>(post);
		}

		public async Task<CommentServiceModel> GetCommentById(Guid id)
		{
			Comment comment = await this._postRepository.GetCommentByIdAsync(id);

			if (comment == null)
				throw new ArgumentException("The comment does not exist");

			return this._postMapper.Map<CommentServiceModel>(comment);
		}

		//Update
		public async Task<bool> UpdatePost(UpdatePostServiceModel postServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(postServiceModel.IssuerId))
				throw new ArgumentException("Comment does not exist!");

			Post post = this._postMapper.Map<Post>(postServiceModel);
			return await this._postRepository.EditAsync(postServiceModel.Id, post);
		}

		public async Task<bool> UpdateComment(UpdateCommentServiceModel commentServiceModel)
		{
			if (!await this._postRepository.DoesCommentExist(commentServiceModel.Id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = this._postMapper.Map<Comment>(commentServiceModel);
			bool result = await this._postRepository.EditCommentAsync(comment);

			return result;
		}

		//Delete
		public async Task<bool> DeletePost(Guid id)
		{
			Post post = await this._postRepository.GetByIdAsync(id);
			return await this._postRepository.DeleteAsync(post);
		}

		public async Task<bool> DeleteComment(Guid id)
		{
			if (!await this._postRepository.DoesCommentExist(id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = await this._postRepository.GetCommentByIdAsync(id);
			bool result = await this._postRepository.DeleteCommentAsync(comment);

			return result;
		}

		//Validate
		public async Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData)
		{
			Comment comment = await this._postRepository.GetCommentByIdAsync(commentId);
			User user = await this.GetUserForValidation(rawTokenData);

			if (comment.IssuerId != user.Id)
				return false;

			return true;
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
	}
}
