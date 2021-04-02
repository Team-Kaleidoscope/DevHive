using System;
using AutoMapper;
using DevHive.Web.Models.ProfilePicture;
using DevHive.Services.Models.ProfilePicture;

namespace DevHive.Web.Configurations.Mapping
{
	public class ProfilePictureMappings : Profile
	{
		public ProfilePictureMappings()
		{
			CreateMap<ProfilePictureWebModel, ProfilePictureServiceModel>()
				.ForMember(dest => dest.ProfilePictureFormFile, src => src.MapFrom(p => p.Picture));
		}
	}
}
