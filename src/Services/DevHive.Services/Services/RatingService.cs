using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Post.Rating;

namespace DevHive.Services.Services
{
	public class RatingService : IRatingService
	{
		private readonly IPostRepository _postRepository;
		private readonly IUserRepository _userRepository;
		private readonly IRatingRepository _ratingRepository;
		private readonly IMapper _mapper;

		public RatingService(IPostRepository postRepository, IRatingRepository ratingRepository, IUserRepository userRepository, IMapper mapper)
		{
			this._postRepository = postRepository;
			this._ratingRepository = ratingRepository;
			this._userRepository = userRepository;
			this._mapper = mapper;
		}

		#region Create
		public async Task<Guid> RatePost(CreateRatingServiceModel createRatingServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(createRatingServiceModel.PostId))
				throw new ArgumentException("Post does not exist!");

			if (await this._ratingRepository.UserRatedPost(createRatingServiceModel.UserId, createRatingServiceModel.PostId))
				throw new ArgumentException("User already rated the post!");

			Rating rating = this._mapper.Map<Rating>(createRatingServiceModel);

			User user = await this._userRepository.GetByIdAsync(createRatingServiceModel.UserId);
			Post post = await this._postRepository.GetByIdAsync(createRatingServiceModel.PostId);
			rating.User = user;
			rating.Post = post;

			bool success = await this._ratingRepository.AddAsync(rating);

			if (success)
			{
				Rating newRating = await this._ratingRepository.GetRatingByUserAndPostId(rating.User.Id, rating.Post.Id);

				return newRating.Id;
			}
			else
				return Guid.Empty;
		}
		#endregion

		#region Read
		public async Task<ReadRatingServiceModel> GetRatingById(Guid ratingId)
		{
			Rating rating = await this._ratingRepository.GetByIdAsync(ratingId) ??
				throw new ArgumentException("The rating does not exist");

			User user = await this._userRepository.GetByIdAsync(rating.User.Id) ??
				throw new ArgumentException("The user does not exist");

			ReadRatingServiceModel readRatingServiceModel = this._mapper.Map<ReadRatingServiceModel>(rating);
			readRatingServiceModel.UserId = user.Id;

			return readRatingServiceModel;
		}

		public async Task<bool> HasUserRatedThisPost(Guid userId, Guid postId)
		{
			return await this._ratingRepository
				.UserRatedPost(userId, postId);
		}
		#endregion

		#region Update
		public async Task<ReadRatingServiceModel> UpdateRating(UpdateRatingServiceModel updateRatingServiceModel)
		{
			Rating rating = await this._ratingRepository.GetByIdAsync(updateRatingServiceModel.Id) ??
				throw new ArgumentException("Rating does not exist!");

			User user = await this._userRepository.GetByIdAsync(updateRatingServiceModel.UserId) ??
				throw new ArgumentException("User does not exist!");

			if (!await this._ratingRepository.UserRatedPost(updateRatingServiceModel.UserId, updateRatingServiceModel.PostId))
				throw new ArgumentException("User has not rated the post!");

			rating.User = user;
			rating.IsLike = updateRatingServiceModel.IsLike;

			bool result = await this._ratingRepository.EditAsync(updateRatingServiceModel.Id, rating);

			if (result)
			{
				ReadRatingServiceModel readRatingServiceModel = this._mapper.Map<ReadRatingServiceModel>(rating);
				return readRatingServiceModel;
			}
			else
				return null;
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteRating(Guid ratingId)
		{
			if (!await this._ratingRepository.DoesRatingExist(ratingId))
				throw new ArgumentException("Rating does not exist!");

			Rating rating = await this._ratingRepository.GetByIdAsync(ratingId);
			return await this._ratingRepository.DeleteAsync(rating);
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
		public async Task<bool> ValidateJwtForRating(Guid commentId, string rawTokenData)
		{
			Rating rating = await this._ratingRepository.GetByIdAsync(commentId) ??
				throw new ArgumentException("Rating does not exist!");
			User user = await this.GetUserForValidation(rawTokenData);

			//If user made the comment
			if (rating.User.Id == user.Id)
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

			Guid jwtUserId = Guid.Parse(this.GetClaimTypeValues("ID", jwt.Claims).First());

			User user = await this._userRepository.GetByIdAsync(jwtUserId) ??
				throw new ArgumentException("User does not exist!");

			return user;
		}

		/// <summary>
		/// Returns all values from a given claim type
		/// </summary>
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
