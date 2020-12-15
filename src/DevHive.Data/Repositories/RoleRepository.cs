using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RoleRepository : IRepository<Role>
	{
		private readonly DbContext _context;

		public RoleRepository(DbContext context)
		{
			this._context = context;
		}

		//Create
		public async Task AddAsync(Role entity)
		{
			await this._context
				.Set<Role>()
				.AddAsync(entity);

			await this._context.SaveChangesAsync();
		}

		//Read
		public async Task<Role> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Role>()
				.FindAsync(id);
				
		}

		//Update
		public async Task EditAsync(Role newEntity)
		{
			this._context
				.Set<Role>()
				.Update(newEntity);

			await this._context.SaveChangesAsync();
		}
		
		//Delete
		public async Task DeleteAsync(Role entity)
		{
			this._context
				.Set<Role>()
				.Remove(entity);

			await this._context.SaveChangesAsync();
		}

		public async Task<bool> DoesNameExist(string name)
		{
			return await this._context
				.Set<Role>()
				.AsNoTracking()
				.AnyAsync(r => r.Name == name);
		}

		public async Task<bool> DoesRoleExist(Guid id)
		{
			return await this._context
				.Set<Role>()
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
	}
}
