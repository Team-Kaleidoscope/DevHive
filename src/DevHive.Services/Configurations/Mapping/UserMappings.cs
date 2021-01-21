using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Identity.User;
using DevHive.Common.Models.Misc;

namespace DevHive.Services.Configurations.Mapping
{
	public class UserMappings : Profile
	{
		public UserMappings()
		{
			CreateMap<UserServiceModel, User>();
			CreateMap<RegisterServiceModel, User>();
			CreateMap<UpdateUserServiceModel, User>()
				.AfterMap((src, dest) => dest.PasswordHash = PasswordModifications.GeneratePasswordHash(src.Password));
			CreateMap<FriendServiceModel, User>();

			CreateMap<User, UserServiceModel>();
			CreateMap<User, UpdateUserServiceModel>()
				.ForMember(x => x.Password, opt => opt.Ignore());
			CreateMap<User, FriendServiceModel>();
		}
	}
}
