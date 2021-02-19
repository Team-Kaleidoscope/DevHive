using AutoMapper;
using DevHive.Services.Models.Message;
using DevHive.Web.Models.Message;

namespace DevHive.Web.Configurations.Mapping
{
	public class MessageMapping : Profile
	{
		public MessageMapping()
		{
			CreateMap<CreateMessageWebModel, CreateMessageServiceModel>();

			CreateMap<ReadMessageServiceModel, ReadMessageWebModel>()
				.ForMember(dest => dest.CreatorFirstName, src => src.MapFrom(p => p.Creator.FirstName))
				.ForMember(dest => dest.CreatorLastName, src => src.MapFrom(p => p.Creator.LastName))
				.ForMember(dest => dest.CreatorUsername, src => src.MapFrom(p => p.Creator.UserName));
		}
	}
}
