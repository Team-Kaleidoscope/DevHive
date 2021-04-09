using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Rating;

namespace DevHive.Services.Configurations.Mapping
{
	public class RatingMappings : Profile
	{
		public RatingMappings()
		{
			CreateMap<CreateRatingServiceModel, Rating>()
				.ForMember(dest => dest.User, src => src.Ignore())
				.ForMember(dest => dest.Post, src => src.Ignore())
				.ForMember(dest => dest.Id, src => src.Ignore());

			CreateMap<Rating, ReadRatingServiceModel>();

			CreateMap<UpdateRatingServiceModel, Rating>();
		}
	}
}
