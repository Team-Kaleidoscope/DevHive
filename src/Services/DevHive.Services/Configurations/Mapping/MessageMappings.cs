using AutoMapper;
using DevHive.Data.Models;
using DevHive.Services.Models.Message;

namespace DevHive.Services.Configurations.Mapping
{
	public class MessageMappings : Profile
	{
		public MessageMappings()
		{
			CreateMap<CreateMessageServiceModel, Message>();

			CreateMap<Message, ReadMessageServiceModel>();
		}
	}
}
