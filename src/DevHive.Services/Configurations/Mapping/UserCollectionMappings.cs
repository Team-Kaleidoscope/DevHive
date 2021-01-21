using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Identity.User;

namespace DevHive.Services.Configurations.Mapping
{
	public class UserCollectionMappings : Profile
	{
		public UserCollectionMappings()
		{
			CreateMap<UpdateUserCollectionServiceModel, User>();
			CreateMap<UpdateUserCollectionServiceModel, Role>();
			CreateMap<UpdateUserCollectionServiceModel, Language>();
			CreateMap<UpdateUserCollectionServiceModel, Technology>();

			CreateMap<User, UpdateUserCollectionServiceModel>();
			CreateMap<Role, UpdateUserCollectionServiceModel>();
			CreateMap<Language, UpdateUserCollectionServiceModel>();
			CreateMap<Technology, UpdateUserCollectionServiceModel>();
		}
	}
}
