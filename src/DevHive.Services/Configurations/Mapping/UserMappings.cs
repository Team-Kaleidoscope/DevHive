using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Identity.User;
using DevHive.Common.Models.Misc;
using DevHive.Data.RelationModels;

namespace DevHive.Services.Configurations.Mapping
{
	public class UserMappings : Profile
	{
		public UserMappings()
		{
			CreateMap<UserServiceModel, User>();
			CreateMap<RegisterServiceModel, User>();
			CreateMap<FriendServiceModel, User>()
				.ForMember(dest => dest.Friends, src => src.Ignore());
			CreateMap<UserFriends, FriendServiceModel>()
				.ForMember(dest => dest.UserName, src => src.MapFrom(p => p.Friend.UserName));
			CreateMap<UpdateUserServiceModel, User>()
				.ForMember(dest => dest.Friends, src => src.Ignore())
				.AfterMap((src, dest) => dest.PasswordHash = PasswordModifications.GeneratePasswordHash(src.Password));
			CreateMap<UpdateFriendServiceModel, User>();

			CreateMap<User, UserServiceModel>()
				.ForMember(dest => dest.Friends, src => src.MapFrom(p => p.Friends));
			CreateMap<User, UpdateUserServiceModel>()
				.ForMember(x => x.Password, opt => opt.Ignore());
			CreateMap<User, FriendServiceModel>();
		}
	}
}
