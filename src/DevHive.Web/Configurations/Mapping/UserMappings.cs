using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Identity.User;
using DevHive.Web.Models.Identity.User;

namespace DevHive.Web.Configurations.Mapping
{
	public class UserMappings : Profile
	{
		public UserMappings()
		{
			CreateMap<LoginWebModel, LoginServiceModel>();
			CreateMap<RegisterWebModel, RegisterServiceModel>();
			CreateMap<UserWebModel, UserServiceModel>();
			CreateMap<UpdateUserWebModel, UpdateUserServiceModel>();

			CreateMap<UserServiceModel, UserWebModel>();

			CreateMap<TokenServiceModel, TokenWebModel>();
		}
	} 
}
