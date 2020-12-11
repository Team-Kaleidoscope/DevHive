using Data.Models.Classes;
using Data.Models.DTOs;
using AutoMapper;

namespace Data.Models.Profiles 
{
	public class UserProfile : Profile
	{
		public UserProfile() 
		{
			CreateMap<UserDTO, User>();
		}
	} 
}
