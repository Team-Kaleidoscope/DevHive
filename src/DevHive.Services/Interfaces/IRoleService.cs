using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Identity;

namespace DevHive.Services.Interfaces
{
	public interface IRoleService
	{
		Task<bool> CreateRole(RoleModel roleServiceModel);

		Task<RoleModel> GetRoleById(Guid id);

		Task<bool> UpdateRole(RoleModel roleServiceModel);
		
		Task<bool> DeleteRole(Guid id);
	}
}