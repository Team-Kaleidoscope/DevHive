using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.User;

namespace DevHive.Services.Configurations.Mapping
{
	public class UserMappings : Profile
	{
		public UserMappings()
		{
			CreateMap<UserServiceModel, User>();
			CreateMap<RegisterServiceModel, User>()
				.ForMember(dest => dest.PasswordHash, src => src.MapFrom(p => p.Password));
			CreateMap<FriendServiceModel, User>()
				.ForMember(dest => dest.Friends, src => src.Ignore());
			CreateMap<UpdateUserServiceModel, User>()
				.ForMember(dest => dest.Friends, src => src.Ignore());
			CreateMap<UpdateFriendServiceModel, User>();

			CreateMap<User, UserServiceModel>()
				.ForMember(dest => dest.ProfilePictureURL, src => src.MapFrom(p => p.ProfilePicture.PictureURL))
				.ForMember(dest => dest.Friends, src => src.MapFrom(p => p.Friends));
			CreateMap<User, UpdateUserServiceModel>()
				.ForMember(dest => dest.ProfilePictureURL, src => src.MapFrom(p => p.ProfilePicture.PictureURL));
			CreateMap<User, FriendServiceModel>();
		}
	}
}
