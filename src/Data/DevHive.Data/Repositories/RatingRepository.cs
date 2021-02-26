using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RatingRepository : BaseRepository<Rating>, IRatingRepository
	{
		private readonly DevHiveContext _context;
		private readonly IPostRepository _postRepository;

		public RatingRepository(DevHiveContext context, IPostRepository postRepository)
			: base(context)
		{
			this._context = context;
			this._postRepository = postRepository;
		}

		public override async Task<Rating> GetByIdAsync(Guid id)
		{
			return await this._context.Rating
				.Include(x => x.User)
				.Include(x => x.Post)
				.FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<List<Rating>> GetRatingsByPostId(Guid postId)
		{
			return await this._context.Rating
				.Where(x => x.Post.Id == postId).ToListAsync();
		}
		public async Task<bool> UserRatedPost(Guid userId, Guid postId)
		{
			return await this._context.Rating
				.Where(x => x.Post.Id == postId)
				.AnyAsync(x => x.User.Id == userId);
		}
		public async Task<Rating> GetRatingByUserAndPostId(Guid userId, Guid postId)
		{
			return await this._context.Rating
				.FirstOrDefaultAsync(x => x.Post.Id == postId && x.User.Id == userId);
		}

		public async Task<bool> DoesRatingExist(Guid id)
		{
			return await this._context.Rating
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
	}
}

