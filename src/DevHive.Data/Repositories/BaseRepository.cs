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
		}

		public virtual async Task<bool> AddAsync(TEntity entity)
		{
			await this._context
				.Set<TEntity>()
				.AddAsync(entity);

			return await this.SaveChangesAsync();
		}

		public virtual async Task<TEntity> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<TEntity>()
				.FindAsync(id);
		}

		public virtual async Task<bool> EditAsync(Guid id, TEntity newEntity)
		{
			var entry = this._context.Entry(newEntity);
			if (entry.State == EntityState.Detached)
				this._context.Attach(newEntity);

			entry.State = EntityState.Modified;

			return await this.SaveChangesAsync();
		}

		public virtual async Task<bool> DeleteAsync(TEntity entity)
		{
			this._context.Remove(entity);

			return await this.SaveChangesAsync();
		}

		public virtual async Task<bool> SaveChangesAsync()
		{
			int result = await _context.SaveChangesAsync();

			return result >= 1;
		}
	}
}
