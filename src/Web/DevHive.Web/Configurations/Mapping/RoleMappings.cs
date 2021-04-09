using AutoMapper;
using DevHive.Web.Models.Role;
using DevHive.Services.Models.Role;

namespace DevHive.Web.Configurations.Mapping
{
	public class RoleMappings : Profile
	{
		public RoleMappings()
		{
			CreateMap<CreateRoleWebModel, CreateRoleServiceModel>();
			CreateMap<UpdateRoleWebModel, UpdateRoleServiceModel>()
				.ForMember(src => src.Id, dest => dest.Ignore());
			CreateMap<RoleWebModel, RoleServiceModel>();

			CreateMap<CreateRoleServiceModel, CreateRoleWebModel>();
			CreateMap<UpdateRoleServiceModel, UpdateRoleWebModel>();
			CreateMap<RoleServiceModel, RoleWebModel>();
		}
	}
}
