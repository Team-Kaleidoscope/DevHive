using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RoleRepository : BaseRepository<Role>, IRoleRepository
	{
		private readonly DevHiveContext _context;

		public RoleRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		#region Read
		public async Task<Role> GetByNameAsync(string name)
		{
			return await this._context.Roles
				.FirstOrDefaultAsync(x => x.Name == name);
		}
		#endregion

		public override async Task<bool> EditAsync(Guid id, Role newEntity)
		{
			Role role = await this.GetByIdAsync(id);

			this._context
				.Entry(role)
				.CurrentValues
				.SetValues(newEntity);

			return await this.SaveChangesAsync(this._context);
		}

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
