using DevHive.Data.Interfaces;
using DevHive.Data.Models;

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
	}
}
