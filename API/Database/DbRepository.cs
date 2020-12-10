using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using Models.Classes;

namespace API.Database
{
	public class DbRepository<TEntity> : IRepository<TEntity>
		where TEntity : class
	{
		private readonly DbContext _context;
		public DbRepository(DbContext context)
		{
			_context = context;
		}

		//Create
		public async Task AddAsync(TEntity entity)
		{
			await this._context
				.Set<TEntity>()
				.AddAsync(entity);

			await this._context.SaveChangesAsync();
		}

		//Read
		public async Task<TEntity> FindByIdAsync(object id)
		{
			return await this._context
				.Set<TEntity>()
				.FindAsync(id);
		}

		public IEnumerable<TEntity> Query(int count)
		{
			return this._context
				.Set<TEntity>()
				.AsNoTracking()
				.Take(count)
				.AsEnumerable();
		}

		//Update
		public async Task EditAsync(object id, TEntity newEntity)
		{
			//Set the Id property to the given id
			TEntity entity = await FindByIdAsync(id);

			this._context.Entry(entity)
				.CurrentValues
				.SetValues(newEntity);

			await this._context.SaveChangesAsync();
		}

		//Delete
		public async Task DeleteAsync(object id)
		{
			TEntity entity = await FindByIdAsync(id);

			this._context.Set<TEntity>().Remove(entity);

			await this._context.SaveChangesAsync();
		}
	}
}