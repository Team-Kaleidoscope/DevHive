using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Rating;

namespace DevHive.Services.Interfaces
{
	public interface IRatingService
	{
		Task<ReadRatingServiceModel> RatePost(CreateRatingServiceModel ratePostServiceModel);

		bool HasUserRatedThisPost(User user, Post post);
	}
}
