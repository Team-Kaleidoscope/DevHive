using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Services.Interfaces
{
	public interface IRoleService
	{
		Task<Guid> CreateRole(CreateRoleServiceModel roleServiceModel);

		Task<ReadRoleServiceModel> GetRoleById(Guid id);

		Task<bool> UpdateRole(UpdateRoleServiceModel roleServiceModel);

		Task<bool> DeleteRole(Guid id);
	}
}
