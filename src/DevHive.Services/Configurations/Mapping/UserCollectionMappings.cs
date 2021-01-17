using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Identity.User;

namespace DevHive.Services.Configurations.Mapping
{
	public class UserCollectionMappings : Profile
	{
		public UserCollectionMappings()
		{
			CreateMap<UpdateUserCollectionServiceModel, User>()
				.ForMember(up => up.UserName, u => u.MapFrom(src => src.Name));
			CreateMap<UpdateUserCollectionServiceModel, Role>()
				.ForMember(r => r.Name, u => u.MapFrom(src => src.Name));
			CreateMap<UpdateUserCollectionServiceModel, Language>()
				.ForMember(r => r.Name, u => u.MapFrom(src => src.Name));
			CreateMap<UpdateUserCollectionServiceModel, Technology>()
				.ForMember(r => r.Name, u => u.MapFrom(src => src.Name));
		}
	}
}
