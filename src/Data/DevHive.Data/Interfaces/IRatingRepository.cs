using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface IRatingRepository : IRepository<Rating>
	{
		Task<Rating> GetRatingByPostId(Guid postId);
		Task<bool> UserRatedPost(Guid userId, Guid postId);
	}
}
