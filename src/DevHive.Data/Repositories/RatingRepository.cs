using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RatingRepository : BaseRepository<Rating>, IRatingRepository
	{
		private readonly DevHiveContext _context;

		public RatingRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		public async Task<Rating> GetByPostId(Guid postId)
		{
			return await this._context.Rating
				.FirstOrDefaultAsync(x => x.Post.Id == postId);
		}

		public async Task<Tuple<int, int>> GetRating(Guid postId)
		{
			Rating rating = await this.GetByPostId(postId);

			return new Tuple<int, int>(rating.Likes, rating.Dislikes);
		}

		public async Task<bool> HasUserRatedThisPost(Guid userId, Guid postId)
		{
			throw new NotImplementedException();
		}
	}
}
