using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Rating;

namespace DevHive.Services.Interfaces
{
	public interface IRateService
	{
		Task<ReadPostRatingServiceModel> RatePost(RatePostServiceModel ratePostServiceModel);

		bool HasUserRatedThisPost(User user, Post post);
	}
}
