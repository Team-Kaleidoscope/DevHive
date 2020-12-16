using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Common.Models.Data;
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
		public async Task<bool> AddAsync(Role entity)
		{
			await this._context
				.Set<Role>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Read
		public async Task<Role> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Role>()
				.FindAsync(id);
				
		}

		//Update
		public async Task<bool> EditAsync(Role newEntity)
		{
			Role role = await this.GetByIdAsync(newEntity.Id);

			this._context
				.Entry(role)
				.CurrentValues
				.SetValues(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		
		//Delete
		public async Task<bool> DeleteAsync(Role entity)
		{
			this._context
				.Set<Role>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
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
