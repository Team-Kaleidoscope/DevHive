using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface IRatingRepository : IRepository<Rating>
	{
		Task<List<Rating>> GetRatingsByPostId(Guid postId);
		Task<bool> UserRatedPost(Guid userId, Guid postId);
		Task<Rating> GetRatingByUserAndPostId(Guid userId, Guid postId);

		Task<bool> DoesRatingExist(Guid id);
	}
}
