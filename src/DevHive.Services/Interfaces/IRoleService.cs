using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Services.Interfaces
{
    public interface IRoleService
	{
		Task<Guid> CreateRole(RoleServiceModel roleServiceModel);

		Task<RoleServiceModel> GetRoleById(Guid id);

		Task<bool> UpdateRole(RoleServiceModel roleServiceModel);

		Task<bool> DeleteRole(Guid id);
	}
}
