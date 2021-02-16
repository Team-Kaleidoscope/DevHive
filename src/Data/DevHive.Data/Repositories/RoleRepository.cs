using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RoleRepository : BaseRepository<Role>, IRoleRepository
	{
		private readonly RoleManager<Role> _roleManager;

		public RoleRepository(DevHiveContext context, RoleManager<Role> roleManager)
			: base(context)
		{
			this._roleManager = roleManager;
		}

		#region Create
		public override async Task<bool> AddAsync(Role entity)
		{
			IdentityResult result = await this._roleManager.CreateAsync(entity);

			return result.Succeeded;
		}
		#endregion

		#region Read
		public async Task<Role> GetByNameAsync(string name)
		{
			return await this._roleManager.FindByNameAsync(name);
		}
		#endregion

		public override async Task<bool> EditAsync(Guid id, Role newEntity)
		{
			newEntity.Id = id;
			IdentityResult result = await this._roleManager.UpdateAsync(newEntity);

			return result.Succeeded;
		}

		#region Validations
		public async Task<bool> DoesNameExist(string name)
		{
			return await this._roleManager.RoleExistsAsync(name);
		}

		public async Task<bool> DoesRoleExist(Guid id)
		{
			return await this._roleManager.Roles.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
