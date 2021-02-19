using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class MessageRepository : BaseRepository<Message>, IMessageRepository
	{
		private readonly DevHiveContext _context;

		public MessageRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		public async Task<Message> GetMessageByCreatorAndTimeCreatedAsync(Guid creatorId, DateTime timeCreated)
		{
			return await this._context.Message
				.FirstOrDefaultAsync(p => p.Creator.Id == creatorId &&
					p.TimeCreated == timeCreated);
		}
	}
}
