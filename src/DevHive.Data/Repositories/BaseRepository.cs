using System;
using System.Threading.Tasks;
using DevHive.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class BaseRepository<TEntity> : IRepository<TEntity>
		where TEntity : class
	{
		private readonly DbContext _context;

		public BaseRepository(DbContext context)
		{
			this._context = context;
			this._context.ChangeTracker.AutoDetectChangesEnabled = false;
		}

		public virtual async Task<bool> AddAsync(TEntity entity)
		{
			await this._context
				.Set<TEntity>()
				.AddAsync(entity);

			return await this.SaveChangesAsync(_context);
		}

		public virtual async Task<TEntity> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<TEntity>()
				.FindAsync(id);
		}

		public virtual async Task<bool> EditAsync(TEntity newEntity)
		{
			// Old way(backup)
			// User user = await this._context.Users
			// 	.FirstOrDefaultAsync(x => x.Id == entity.Id);

			// this._context.Update(user);
			// this._context.Entry(entity).CurrentValues.SetValues(entity);

			this._context
				.Set<TEntity>()
				.Update(newEntity);

			return await this.SaveChangesAsync(_context);
		}

		public virtual async Task<bool> DeleteAsync(TEntity entity)
		{
			this._context.Remove(entity);

			return await this.SaveChangesAsync(_context);
		}

		public virtual async Task<bool> SaveChangesAsync(DbContext context)
		{
			int result = await context.SaveChangesAsync();

			return result >= 1;
		}
	}
}
