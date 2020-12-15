using System;
using System.Threading.Tasks;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Identity.Role;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Services.Services
{
	public class RoleService
	{
		private readonly DevHiveContext _context;

		public RoleService(DevHiveContext context)
		{
			this._context = context;
		}

		public Task<IActionResult> CreateRole(RoleServiceModel roleServiceModel)
		{
			throw new NotImplementedException();
		}

		public Task<IActionResult> GetRoleById(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<IActionResult> UpdateRole(RoleServiceModel roleServiceModel)
		{
			throw new NotImplementedException();
		}

		public Task<IActionResult> DeleteRole(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
