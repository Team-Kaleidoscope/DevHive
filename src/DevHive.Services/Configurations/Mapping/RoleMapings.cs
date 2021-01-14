using DevHive.Data.Models;
using AutoMapper;
using DevHive.Common.Models.Identity;

namespace DevHive.Services.Configurations.Mapping
{
	public class RoleMappings : Profile
	{
		public RoleMappings()
		{
			CreateMap<RoleModel, Role>();
			CreateMap<Role, RoleModel>();
		}
	}
}
