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
	public class RateService : IRateService
	{
		private readonly IPostRepository _postRepository;
		private readonly IUserRepository _userRepository;
		private readonly IRatingRepository _ratingRepository;
		private readonly IMapper _mapper;

		public RateService(IPostRepository postRepository, IRatingRepository ratingRepository, IUserRepository userRepository, IMapper mapper)
		{
			this._postRepository = postRepository;
			this._ratingRepository = ratingRepository;
			this._userRepository = userRepository;
			this._mapper = mapper;
		}

		public async Task<ReadPostRatingServiceModel> RatePost(RatePostServiceModel ratePostServiceModel)
		{
			throw new NotImplementedException();
			// if (!await this._postRepository.DoesPostExist(ratePostServiceModel.PostId))
			// throw new ArgumentException("Post does not exist!");

			// if (!await this._userRepository.DoesUserExistAsync(ratePostServiceModel.UserId))
			// 	throw new ArgumentException("User does not exist!");

			// Post post = await this._postRepository.GetByIdAsync(ratePostServiceModel.PostId);
			// User user = await this._userRepository.GetByIdAsync(ratePostServiceModel.UserId);

			// if (this.HasUserRatedThisPost(user, post))
			// 	throw new ArgumentException("You can't rate the same post more then one(duh, amigo)");

			// this.Rate(user, post, ratePostServiceModel.Liked);

			// bool success = await this._ratingRepository.EditAsync(post.Rating.Id, post.Rating);
			// if (!success)
			// 	throw new InvalidOperationException("Unable to rate the post!");

			// Rating newRating = await this._ratingRepository.GetByIdAsync(post.Rating.Id);
			// return this._mapper.Map<ReadPostRatingServiceModel>(newRating);
		}

		public async Task<ReadPostRatingServiceModel> RemoveUserRateFromPost(Guid userId, Guid postId)
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

		private void Rate(User user, Post post, bool liked)
		{
			throw new NotImplementedException();
			// if (liked)
			// 	post.Rating.Rate++;
			// else
			// 	post.Rating.Rate--;

			// post.Rating.UsersThatRated.Add(user);
		}
	}
}
