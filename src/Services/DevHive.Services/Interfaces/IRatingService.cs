using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Rating;

namespace DevHive.Services.Interfaces
{
	public interface IRatingService
	{
		Task<Guid> RatePost(CreateRatingServiceModel createRatingServiceModel);

		Task<ReadRatingServiceModel> GetRatingById(Guid ratingId);
		Task<ReadRatingServiceModel> GetUserRateFromPost(Guid userId, Guid postId);
		Task<bool> HasUserRatedThisPost(Guid userId, Guid postId);

		Task<ReadRatingServiceModel> RemoveUserRateFromPost(Guid userId, Guid postId);
	}
}
