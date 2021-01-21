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

		#region Create
		public async Task<bool> AddCommentAsync(Comment entity)
		{
			await this._context.Comments
				.AddAsync(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read
		public async Task<Post> GetPostByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated)
		{
			return await this._context.Posts
				.FirstOrDefaultAsync(p => p.IssuerId == issuerId &&
					p.TimeCreated == timeCreated);
		}

		public async Task<Comment> GetCommentByIdAsync(Guid id)
		{
			return await this._context.Comments
				.FindAsync(id);
		}

		public async Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated)
		{
			return await this._context.Comments
				.FirstOrDefaultAsync(p => p.IssuerId == issuerId &&
					p.TimeCreated == timeCreated);
		}
		#endregion

		#region Update
		public async Task<bool> EditCommentAsync(Comment newEntity)
		{
			this._context.Comments
				.Update(newEntity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteCommentAsync(Comment entity)
		{
			this._context.Comments
				.Remove(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Validations
		public async Task<bool> DoesPostExist(Guid postId)
		{
			return await this._context.Posts
				.AsNoTracking()
				.AnyAsync(r => r.Id == postId);
		}

		public async Task<bool> DoesCommentExist(Guid id)
		{
			return await this._context.Comments
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
