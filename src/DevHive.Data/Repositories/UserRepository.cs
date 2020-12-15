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
		/*
		public User FindByUsername(string username)
		{
			return this._dbRepository.DbSet
				.FirstOrDefault(usr => usr.UserName == username);
		}

		public bool DoesUsernameExist(string username)
		{
			return this._dbRepository.DbSet
				.Any(x => x.UserName == username);
		}

		public bool DoesUserExist(Guid id)
		{
			return this._dbRepository.DbSet
				.Any(x => x.Id == id);
		}
	
		public bool HasThisUsername(Guid id, string username)
		{
			return this._dbRepository.DbSet
				.Any(x => x.Id == id &&
					x.UserName == username);
		} */

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

		public Task<User> FindByIdAsync(Guid id)
		{
			throw new System.NotImplementedException();
		}

		public Task EditAsync(object id, User newEntity)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync(object id)
		{
			throw new System.NotImplementedException();
		}
	}
}
