using AutoMapper;
using DevHive.Services.Models;
using DevHive.Web.Models.Comment;
using DevHive.Web.Models.Feed;

namespace DevHive.Web.Configurations.Mapping
{
	public class FeedMappings : Profile
	{
		public FeedMappings()
		{
			CreateMap<GetPageWebModel, GetPageServiceModel>()
				.ForMember(dest => dest.FirstRequestIssued, src => src.MapFrom(p => p.FirstPageTimeIssued));

			CreateMap<ReadPageServiceModel, ReadPageWebModel>();
		}
	}
}
