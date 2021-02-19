using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Repositories
{
	public class ChatRepository : BaseRepository<Chat>
	{
		private readonly DevHiveContext _context;

		public ChatRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}
	}
}
