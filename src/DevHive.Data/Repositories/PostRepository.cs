using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class PostRepository : BaseRepository<Post>, IPostRepository
	{
		private readonly DevHiveContext _context;

		public PostRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		#region Read
		public override async Task<Post> GetByIdAsync(Guid id)
		{
			return await this._context.Posts
					.Include(x => x.Comments)
					.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Post> GetPostByCreatorAndTimeCreatedAsync(Guid creatorId, DateTime timeCreated)
		{
			return await this._context.Posts
				.FirstOrDefaultAsync(p => p.CreatorId == creatorId &&
					p.TimeCreated == timeCreated);
		}
		#endregion

		#region Validations
		public async Task<bool> DoesPostExist(Guid postId)
		{
			return await this._context.Posts
				.AsNoTracking()
				.AnyAsync(r => r.Id == postId);
		}
		#endregion
	}
}
