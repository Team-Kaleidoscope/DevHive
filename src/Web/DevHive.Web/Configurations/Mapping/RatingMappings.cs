using AutoMapper;
using DevHive.Services.Models.Post.Rating;
using DevHive.Web.Models.Rating;

namespace DevHive.Web.Configurations.Mapping
{
	public class RatingMappings : Profile
	{
		public RatingMappings()
		{
			CreateMap<RatePostWebModel, CreateRatingServiceModel>();

			CreateMap<ReadRatingServiceModel, ReadPostRatingWebModel>();
		}
	}
}
