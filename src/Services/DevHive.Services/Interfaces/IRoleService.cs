using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Role;

namespace DevHive.Services.Interfaces
{
	public interface IRoleService
	{
		Task<Guid> CreateRole(CreateRoleServiceModel createRoleServiceModel);

		Task<RoleServiceModel> GetRoleById(Guid id);

		Task<bool> UpdateRole(UpdateRoleServiceModel updateRoleServiceModel);

		Task<bool> DeleteRole(Guid id);
	}
}
