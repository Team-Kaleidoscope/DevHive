using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class UserRepository : IRepository<User>
	{
		private readonly DbContext _context;

		public UserRepository(DbContext context)
		{
			this._context = context;
		}

		public bool DoesUserExist(Guid id)
		{
			return this._context
				.Set<User>()
				.Any(x => x.Id == id);
		}
	
		public bool HasThisUsername(Guid id, string username)
		{
			return this._context
				.Set<User>()
				.Any(x => x.Id == id &&
					x.UserName == username);
		}

		public async Task AddAsync(User entity)
		{
			await this._context
				.Set<User>()
				.AddAsync(entity);

			await this._context.SaveChangesAsync();
		}

		public IEnumerable<User> Query(int count)
		{
			return this._context
				.Set<User>()
				.AsNoTracking()
				.Take(count)
				.AsEnumerable();

		}

		public async Task<User> FindByIdAsync(Guid id)
		{
			return await this._context
				.Set<User>()
				.FindAsync(id);
		}

		public async Task EditAsync(User newEntity)
		{
			this._context
				.Set<User>()
				.Update(newEntity);

			await this._context.SaveChangesAsync();
		}

		public async Task DeleteAsync(User entity)
		{
			this._context
				.Set<User>()
				.Remove(entity);

			await this._context.SaveChangesAsync();
		}
	}
}
