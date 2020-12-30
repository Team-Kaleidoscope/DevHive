using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Common.Models.Data;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class CommentRepository : IRepository<Comment>
	{
		private readonly DevHiveContext _context;

		public CommentRepository(DevHiveContext context)
		{
			this._context = context;
		}

		public async Task<bool> AddAsync(Comment entity)
		{
			await this._context
				.Set<Comment>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		public async Task<Comment> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Comment>()
				.FindAsync(id);
		}


		public async Task<bool> EditAsync(Comment newEntity)
		{
			this._context
				.Set<Comment>()
				.Update(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		
		public async Task<bool> DoesCommentExist(Guid id)
		{
			return await this._context
				.Set<Comment>()
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}

		public async Task<bool> DeleteAsync(Comment entity)
		{
			this._context
				.Set<Comment>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
	}
}