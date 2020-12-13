using Data.Models.Classes;
using Data.Models;
using Data.Models.DTOs.Identity;
using AutoMapper;

namespace Data.Models.Profiles 
{
	public class UserProfile : Profile
	{
		public UserProfile() 
		{
			CreateMap<UserDTO, User>();
			CreateMap<RegisterDTO, User>();
			CreateMap<UpdateUserDTO, User>();
		}
	} 
}
