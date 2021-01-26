using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Services.Configurations.Mapping
{
	public class RoleMappings : Profile
	{
		public RoleMappings()
		{
			CreateMap<CreateRoleServiceModel, Role>();
			CreateMap<RoleServiceModel, Role>();
			CreateMap<UpdateRoleServiceModel, Role>();

			CreateMap<Role, RoleServiceModel>();
			CreateMap<Role, UpdateRoleServiceModel>();
		}
	}
}
