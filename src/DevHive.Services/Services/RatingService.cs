using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Rating;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DevHive.Services.Services
{
	public class RatingService
	{
		private readonly IPostRepository _postRepository;
		private readonly IRatingRepository _ratingRepository;
		private readonly IMapper _mapper;

		public RatingService(IPostRepository postRepository, IRatingRepository ratingRepository, IMapper mapper)
		{
			this._postRepository = postRepository;
			this._ratingRepository = ratingRepository;
			this._mapper = mapper;
		}

		public async Task<ReadRatingServiceModel> RatePost(RatePostServiceModel ratePostServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(ratePostServiceModel.PostId))
				throw new ArgumentNullException("Post does not exist!");

			if (!await this._ratingRepository.HasUserRatedThisPost(ratePostServiceModel.UserId, ratePostServiceModel.PostId))
				throw new ArgumentException("You can't rate the same post more then one(duh, amigo)");

			Post post = await this._postRepository.GetByIdAsync(ratePostServiceModel.PostId);

			Rating rating = post.Rating;
			if (ratePostServiceModel.Liked)
				rating.Likes++;
			else
				rating.Dislikes++;

			bool success = await this._ratingRepository.EditAsync(rating.Id, rating);
			if (!success)
				throw new InvalidOperationException("Unable to rate the post!");

			Rating newRating = await this._ratingRepository.GetByIdAsync(rating.Id);
			return this._mapper.Map<ReadRatingServiceModel>(newRating);
		}

		public async Task<ReadRatingServiceModel> RemoveUserRateFromPost(Guid userId, Guid postId)
		{
			throw new NotImplementedException();
		}
	}
}
