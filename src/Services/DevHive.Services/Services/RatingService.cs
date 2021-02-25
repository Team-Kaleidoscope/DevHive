using System;
using System.Linq;
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

		public async Task<ReadRatingServiceModel> RemoveUserRateFromPost(Guid userId, Guid postId)
		{
			throw new NotImplementedException();
			// Post post = await this._postRepository.GetByIdAsync(postId);
			// User user = await this._userRepository.GetByIdAsync(userId);

			// if (!this.HasUserRatedThisPost(user, post))
			// 	throw new ArgumentException("You haven't rated this post, lmao!");
		}

		public bool HasUserRatedThisPost(User user, Post post)
		{
			throw new NotImplementedException();
			// return post.Rating.UsersThatRated
			// 	.Any(x => x.Id == user.Id);
		}

	}
}
