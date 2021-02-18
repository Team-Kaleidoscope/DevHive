using System;
using System.Threading.Tasks;
using DevHive.Data.Repositories;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Message;

namespace DevHive.Services.Services
{
	public class MessageService : IMessageService
	{
		private readonly MessageRepository _messageRepository;

		public MessageService(MessageRepository messageRepository)
		{
			this._messageRepository = messageRepository;
		}

		public Task<Guid> CreateMessage(CreateMessageServiceModel createMessageServiceModel)
		{
			throw new NotImplementedException();
		}

		public Task<ReadMessageServiceModel> GetMessageById(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
