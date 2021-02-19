using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		Task<Message> GetMessageByCreatorAndTimeCreatedAsync(Guid creatorId, DateTime timeCreated);
	}
}
