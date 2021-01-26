using AutoMapper;
using DevHive.Web.Models.Identity.Role;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Web.Configurations.Mapping
{
	public class RoleMappings : Profile
	{
		public RoleMappings()
		{
			CreateMap<CreateRoleWebModel, CreateRoleServiceModel>();
			CreateMap<UpdateRoleWebModel, UpdateRoleServiceModel>()
				.ForMember(src => src.Id, dest => dest.Ignore());
			CreateMap<RoleWebModel, ReadRoleServiceModel>();

			CreateMap<CreateRoleServiceModel, CreateRoleWebModel>();
			CreateMap<UpdateRoleServiceModel, UpdateRoleWebModel>();
			CreateMap<ReadRoleServiceModel, RoleWebModel>();
		}
	}
}
