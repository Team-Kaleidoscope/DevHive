using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Post;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevHive.Services.Interfaces;
using DevHive.Data.Interfaces.Repositories;
using System.Linq;

namespace DevHive.Services.Services
{
    public class PostService : IPostService
	{
		private readonly ICloudService _cloudService;
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper _postMapper;

		public PostService(ICloudService cloudService, IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, IMapper postMapper)
		{
			this._cloudService = cloudService;
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

			if (createPostServiceModel.Files.Count != 0)
				post.FileUrls = await _cloudService.UploadFilesToCloud(createPostServiceModel.Files);

			post.Creator = await this._userRepository.GetByIdAsync(createPostServiceModel.CreatorId);
			post.TimeCreated = DateTime.Now;

			bool success = await this._postRepository.AddAsync(post);
			if (success)
			{
				Post newPost = await this._postRepository
					.GetPostByCreatorAndTimeCreatedAsync(post.Creator.Id, post.TimeCreated);

				return newPost.Id;
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

			User user = await this._userRepository.GetByIdAsync(post.Creator.Id) ??
				throw new ArgumentException("The user does not exist!");

			ReadPostServiceModel readPostServiceModel = this._postMapper.Map<ReadPostServiceModel>(post);
			readPostServiceModel.CreatorFirstName = user.FirstName;
			readPostServiceModel.CreatorLastName = user.LastName;
			readPostServiceModel.CreatorUsername = user.UserName;
			// readPostServiceModel.Files = await this._cloudService.GetFilesFromCloud(post.FileUrls);

			return readPostServiceModel;
		}
		#endregion

		#region Update
		public async Task<Guid> UpdatePost(UpdatePostServiceModel updatePostServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(updatePostServiceModel.PostId))
				throw new ArgumentException("Post does not exist!");

			Post post = this._postMapper.Map<Post>(updatePostServiceModel);

			if (updatePostServiceModel.Files.Count != 0)
			{
				if (await this._postRepository.DoesPostHaveFiles(updatePostServiceModel.PostId))
				{
					List<string> fileUrls = await this._postRepository.GetFileUrls(updatePostServiceModel.PostId);
					bool success = await _cloudService.RemoveFilesFromCloud(fileUrls);
					if (!success)
						throw new InvalidCastException("Could not delete files from the post!");
				}

				post.FileUrls = await _cloudService.UploadFilesToCloud(updatePostServiceModel.Files) ??
					throw new ArgumentNullException("Unable to upload images to cloud");
			}

			post.Creator = await this._userRepository.GetByIdAsync(updatePostServiceModel.CreatorId);
			post.TimeCreated = DateTime.Now;

			bool result = await this._postRepository.EditAsync(updatePostServiceModel.PostId, post);

			if (result)
				return (await this._postRepository.GetByIdAsync(updatePostServiceModel.PostId)).Id;
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

			if (await this._postRepository.DoesPostHaveFiles(id))
			{
				List<string> fileUrls = await this._postRepository.GetFileUrls(id);
				bool success = await _cloudService.RemoveFilesFromCloud(fileUrls);
				if (!success)
					throw new InvalidCastException("Could not delete files from the post. Please try again");
			}

			return await this._postRepository.DeleteAsync(post);
		}
		#endregion

		#region Validations
		public async Task<bool> ValidateJwtForCreating(Guid userId, string rawTokenData)
		{
			User user = await this.GetUserForValidation(rawTokenData);

			return user.Id == userId;
		}

		public async Task<bool> ValidateJwtForPost(Guid postId, string rawTokenData)
		{
			Post post = await this._postRepository.GetByIdAsync(postId) ??
				throw new ArgumentException("Post does not exist!");
			User user = await this.GetUserForValidation(rawTokenData);

			//If user made the post
			if (post.Creator.Id == user.Id)
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
			if (comment.Creator.Id == user.Id)
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
