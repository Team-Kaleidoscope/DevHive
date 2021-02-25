using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Rating;

namespace DevHive.Services.Configurations.Mapping
{
	public class RatingMappings : Profile
	{
		public RatingMappings()
		{
			CreateMap<CreateRatingServiceModel, Rating>();
		}
	}
}
