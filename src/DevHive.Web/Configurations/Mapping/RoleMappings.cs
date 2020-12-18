using AutoMapper;
using DevHive.Web.Models.Identity.Role;
using DevHive.Common.Models.Identity;

namespace DevHive.Web.Configurations.Mapping
{
    public class RoleMappings : Profile
	{
		public RoleMappings()
		{
			CreateMap<CreateRoleModel, RoleModel>();
			CreateMap<UpdateRoleModel, RoleModel>();
			
			CreateMap<RoleModel, RoleWebModel>();
			CreateMap<RoleWebModel, RoleModel>();
		}
	} 
}
