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
using DevHive.Services.Models.Rating;

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

			rating.User = await this._userRepository.GetByIdAsync(createRatingServiceModel.UserId);
			rating.Post = await this._postRepository.GetByIdAsync(createRatingServiceModel.PostId);

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
			Rating rating = await this._ratingRepository.GetByIdAsync(ratingId);
			if (rating is null)
				return null;

			ReadRatingServiceModel readRatingServiceModel = this._mapper.Map<ReadRatingServiceModel>(rating);
			readRatingServiceModel.UserId = rating.User.Id;

			return readRatingServiceModel;
		}

		public async Task<ReadRatingServiceModel> GetRatingByPostAndUser(Guid userId, Guid postId)
		{
			Rating rating = await this._ratingRepository.GetRatingByUserAndPostId(userId, postId);
			if (rating is null)
				return null;

			ReadRatingServiceModel readRatingServiceModel = this._mapper.Map<ReadRatingServiceModel>(rating);
			readRatingServiceModel.UserId = rating.User.Id;

			return readRatingServiceModel;
		}
		#endregion

		#region Update
		public async Task<ReadRatingServiceModel> UpdateRating(UpdateRatingServiceModel updateRatingServiceModel)
		{
			Rating rating = await this._ratingRepository.GetRatingByUserAndPostId(updateRatingServiceModel.UserId, updateRatingServiceModel.PostId) ??
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
	}
}
