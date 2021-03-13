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
		Task<ReadRatingServiceModel> GetRatingByPostAndUser(Guid userId, Guid postId);


		Task<ReadRatingServiceModel> UpdateRating(UpdateRatingServiceModel updateRatingServiceModel);

		Task<bool> DeleteRating(Guid ratingId);

		Task<bool> HasUserRatedThisPost(Guid userId, Guid postId);
	}
}
