using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Role;

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

			CreateMap<RoleServiceModel, UpdateRoleServiceModel>();
		}
	}
}
