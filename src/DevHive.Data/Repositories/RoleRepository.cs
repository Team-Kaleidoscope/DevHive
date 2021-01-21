using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RoleRepository : BaseRepository, IRoleRepository
	{
		private readonly DevHiveContext _context;

		public RoleRepository(DevHiveContext context)
		{
			this._context = context;
		}

		#region Create
		public async Task<bool> AddAsync(Role entity)
		{
			await this._context.Roles
				.AddAsync(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read
		public async Task<Role> GetByIdAsync(Guid id)
		{
			return await this._context.Roles
				.FindAsync(id);
		}

		public async Task<Role> GetByNameAsync(string name)
		{
			return await this._context.Roles
				.FirstOrDefaultAsync(x => x.Name == name);
		}
		#endregion

		#region Update
		public async Task<bool> EditAsync(Role newEntity)
		{
			Role role = await this.GetByIdAsync(newEntity.Id);

			this._context
				.Entry(role)
				.CurrentValues
				.SetValues(newEntity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteAsync(Role entity)
		{
			this._context.Roles
				.Remove(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Validations
		public async Task<bool> DoesNameExist(string name)
		{
			return await this._context.Roles
				.AsNoTracking()
				.AnyAsync(r => r.Name == name);
		}

		public async Task<bool> DoesRoleExist(Guid id)
		{
			return await this._context.Roles
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
