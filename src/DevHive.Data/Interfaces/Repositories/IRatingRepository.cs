using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface IRatingRepository : IRepository<Rating>
	{
		Task<Rating> GetByPostId(Guid postId);
		Task<Tuple<int, int>> GetRating(Guid postId);

		Task<bool> HasUserRatedThisPost(Guid userId, Guid postId);
	}
}
