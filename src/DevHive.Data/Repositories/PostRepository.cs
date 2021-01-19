using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class PostRepository : BaseRepository, IPostRepository
	{
		private readonly DevHiveContext _context;

		public PostRepository(DevHiveContext context)
		{
			this._context = context;
		}

		//Create
		public async Task<bool> AddAsync(Post post)
		{
			await this._context
				.Set<Post>()
				.AddAsync(post);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> AddCommentAsync(Comment entity)
		{
			await this._context
				.Set<Comment>()
				.AddAsync(entity);

			return await this.SaveChangesAsync(this._context);
		}

		//Read
		public async Task<Post> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Post>()
				.FindAsync(id);
		}

		public async Task<Post> GetPostByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated)
		{
			return await this._context.Posts
				.FirstOrDefaultAsync(p => p.IssuerId == issuerId &&
					p.TimeCreated == timeCreated);
		}

		public async Task<Comment> GetCommentByIdAsync(Guid id)
		{
			return await this._context
				.Set<Comment>()
				.FindAsync(id);
		}

		public async Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated)
		{
			return await this._context.Comments
				.FirstOrDefaultAsync(p => p.IssuerId == issuerId &&
					p.TimeCreated == timeCreated);
		}

		//Update
		public async Task<bool> EditAsync(Post newPost)
		{
			this._context
			.Set<Post>()
			.Update(newPost);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> EditCommentAsync(Comment newEntity)
		{
			this._context
				.Set<Comment>()
				.Update(newEntity);

			return await this.SaveChangesAsync(this._context);
		}

		//Delete
		public async Task<bool> DeleteAsync(Post post)
		{
			this._context
				.Set<Post>()
				.Remove(post);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> DeleteCommentAsync(Comment entity)
		{
			this._context
				.Set<Comment>()
				.Remove(entity);

			return await this.SaveChangesAsync(this._context);
		}

		#region Validations

		public async Task<bool> DoesPostExist(Guid postId)
		{
			return await this._context
				.Set<Post>()
				.AsNoTracking()
				.AnyAsync(r => r.Id == postId);
		}

		public async Task<bool> DoesCommentExist(Guid id)
		{
			return await this._context
				.Set<Comment>()
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
