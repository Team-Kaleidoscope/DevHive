using AutoMapper;
using DevHive.Web.Models.Identity.Role;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Web.Configurations.Mapping
{
    public class RoleMappings : Profile
	{
		public RoleMappings()
		{
			CreateMap<CreateRoleWebModel, RoleServiceModel>();
			CreateMap<UpdateRoleWebModel, RoleServiceModel>();
		}
	} 
}
