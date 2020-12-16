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

		//Create
		public async Task AddAsync(User entity)
		{
			await this._context
				.Set<User>()
				.AddAsync(entity);

			await this._context.SaveChangesAsync();
		}
		
		//Read
		public IEnumerable<User> QueryAll()
		{
			return this._context
				.Set<User>()
				.AsNoTracking()
				.AsEnumerable();
		}

		public async Task<User> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<User>()
				.FindAsync(id);
		}

		public async Task<User> GetByUsername(string username)
		{
			return await this._context
				.Set<User>()
				.FirstOrDefaultAsync(x => x.UserName == username);
		}

		//Update
		public async Task EditAsync(User newEntity)
		{
			User user = await this.GetByIdAsync(newEntity.Id);

			user.UserName = newEntity.UserName;
			user.FirstName = newEntity.FirstName;
			user.LastName = newEntity.LastName;
			user.ProfilePicture = newEntity.ProfilePicture;
			user.Role = newEntity.Role;

			this._context.Update(user);

			await this._context.SaveChangesAsync();
		}

		//Delete
		public async Task DeleteAsync(User entity)
		{
			this._context
				.Set<User>()
				.Remove(entity);

			await this._context.SaveChangesAsync();
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
