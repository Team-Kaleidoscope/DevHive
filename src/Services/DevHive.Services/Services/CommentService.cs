using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Constants;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Comment;

namespace DevHive.Services.Services
{
	public class CommentService : ICommentService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IMapper _postMapper;

		public CommentService(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, IMapper postMapper)
		{
			this._userRepository = userRepository;
			this._postRepository = postRepository;
			this._commentRepository = commentRepository;
			this._postMapper = postMapper;
		}

		#region Create
		public async Task<Guid> AddComment(CreateCommentServiceModel createCommentServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(createCommentServiceModel.PostId))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Post));

			Comment comment = this._postMapper.Map<Comment>(createCommentServiceModel);
			comment.TimeCreated = DateTime.Now;

			comment.Creator = await this._userRepository.GetByIdAsync(createCommentServiceModel.CreatorId);
			comment.Post = await this._postRepository.GetByIdAsync(createCommentServiceModel.PostId);

			bool success = await this._commentRepository.AddAsync(comment);
			if (success)
			{
				Comment newComment = await this._commentRepository
					.GetCommentByIssuerAndTimeCreatedAsync(comment.Creator.Id, comment.TimeCreated);

				return newComment.Id;
			}
			else
				return Guid.Empty;
		}
		#endregion

		#region Read
		public async Task<ReadCommentServiceModel> GetCommentById(Guid id)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(id) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Comment));

			User user = await this._userRepository.GetByIdAsync(comment.Creator.Id) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User));

			ReadCommentServiceModel readCommentServiceModel = this._postMapper.Map<ReadCommentServiceModel>(comment);
			readCommentServiceModel.IssuerFirstName = user.FirstName;
			readCommentServiceModel.IssuerLastName = user.LastName;
			readCommentServiceModel.IssuerUsername = user.UserName;

			return readCommentServiceModel;
		}
		#endregion

		#region Update
		public async Task<Guid> UpdateComment(UpdateCommentServiceModel updateCommentServiceModel)
		{
			if (!await this._commentRepository.DoesCommentExist(updateCommentServiceModel.CommentId))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Comment));

			Comment comment = this._postMapper.Map<Comment>(updateCommentServiceModel);
			comment.TimeCreated = DateTime.Now;

			comment.Creator = await this._userRepository.GetByIdAsync(updateCommentServiceModel.CreatorId);
			comment.Post = await this._postRepository.GetByIdAsync(updateCommentServiceModel.PostId);

			bool result = await this._commentRepository.EditAsync(updateCommentServiceModel.CommentId, comment);

			if (result)
				return (await this._commentRepository.GetByIdAsync(updateCommentServiceModel.CommentId)).Id;
			else
				return Guid.Empty;
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteComment(Guid id)
		{
			if (!await this._commentRepository.DoesCommentExist(id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Comment));

			Comment comment = await this._commentRepository.GetByIdAsync(id);
			return await this._commentRepository.DeleteAsync(comment);
		}
		#endregion

		#region Validations
		/// <summary>
		/// Checks whether the user Id in the token and the given user Id match
		/// </summary>
		public async Task<bool> ValidateJwtForCreating(Guid userId, string rawTokenData)
		{
			User user = await this.GetUserForValidation(rawTokenData);

			return user.Id == userId;
		}

		/// <summary>
		/// Checks whether the comment, gotten with the commentId,
		/// is made by the user in the token
		/// or if the user in the token is an admin
		/// </summary>
		public async Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(commentId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Comment));
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

		/// <summary>
		/// Returns the user, via their Id in the token
		/// </summary>
		private async Task<User> GetUserForValidation(string rawTokenData)
		{
			JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			Guid jwtUserId = Guid.Parse(GetClaimTypeValues("ID", jwt.Claims).First());

			User user = await this._userRepository.GetByIdAsync(jwtUserId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User));

			return user;
		}

		/// <summary>
		/// Returns all values from a given claim type
		/// </summary>
		private static List<string> GetClaimTypeValues(string type, IEnumerable<Claim> claims)
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

