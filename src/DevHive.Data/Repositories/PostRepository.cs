using System;
using System.Collections.Generic;
using System.Linq;
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
				.FirstOrDefaultAsync(p => p.Creator.Id == creatorId &&
					p.TimeCreated == timeCreated);
		}

		public async Task<List<string>> GetFileUrls(Guid postId)
		{
			return (await this.GetByIdAsync(postId)).FileUrls;
		}
		#endregion

		#region Validations
		public async Task<bool> DoesPostExist(Guid postId)
		{
			return await this._context.Posts
				.AsNoTracking()
				.AnyAsync(r => r.Id == postId);
		}

		public async Task<bool> DoesPostHaveFiles(Guid postId)
		{
			return await this._context.Posts
				.AsNoTracking()
				.Where(x => x.Id == postId)
				.Select(x => x.FileUrls)
				.AnyAsync();
		}
		#endregion
	}
}
