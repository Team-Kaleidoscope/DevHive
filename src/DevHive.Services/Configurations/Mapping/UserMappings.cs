using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Identity.User;

namespace DevHive.Services.Configurations.Mapping
{
	public class UserMappings : Profile
	{
		public UserMappings()
		{
			CreateMap<UserServiceModel, User>();
			CreateMap<RegisterServiceModel, User>();
			CreateMap<UpdateUserServiceModel, User>();
			CreateMap<UpdateUserCollectionServiceModel, User>()
				.ForMember(up => up.UserName, u => u.MapFrom(src => src.Name));


			CreateMap<User, UserServiceModel>();
		}
	}
}
