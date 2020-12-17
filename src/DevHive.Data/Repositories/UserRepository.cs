using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Common.Models.Data;
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

		//Create
		public async Task<bool> AddAsync(User entity)
		{
			await this._context
				.Set<User>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		
		//Read
		public IEnumerable<User> QueryAll()
		{
			return this._context
				.Set<User>()
				.Include(x => x.Roles)
				.AsNoTracking()
				.AsEnumerable();
		}

		public async Task<User> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<User>()
				.Include(x => x.Roles)
				// To also return the roles, you need to include the roles table, 
				// but then you loose FindAsync, because there is id of role and id of user
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<User> GetByUsername(string username)
		{
			return await this._context
				.Set<User>()
				.Include(u => u.Roles)
				.FirstOrDefaultAsync(x => x.UserName == username);
		}

		//Update
		public async Task<bool> EditAsync(User newEntity)
		{
			User user = await this.GetByIdAsync(newEntity.Id);

			this._context
				.Entry(user)
				.CurrentValues
				.SetValues(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Delete
		public async Task<bool> DeleteAsync(User entity)
		{
			this._context
				.Set<User>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
	
		//Validations
		public bool DoesUserExist(Guid id)
		{
			return this._context
				.Set<User>()
				.Any(x => x.Id == id);
		}

		public Task<bool> IsUsernameValid(string username)
		{
			return this._context
				.Set<User>()
				.AnyAsync(u => u.UserName == username);
		}

		public bool DoesUserHaveThisUsername(Guid id, string username)
		{
			return this._context
				.Set<User>()
				.Any(x => x.Id == id &&
					x.UserName == username);
		}

		public async Task<bool> DoesUsernameExist(string username)
		{
			return await this._context
				.Set<User>()
				.AsNoTracking()
				.AnyAsync(u => u.UserName == username);
		}

		public async Task<bool> DoesEmailExist(string email)
		{
			return await this._context
				.Set<User>()
				.AsNoTracking()
				.AnyAsync(u => u.Email == email);
		}
	}
}
