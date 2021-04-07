using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Repositories
{
	public class FriendshipRepository : BaseRepository<Friendship>
	{
		private readonly DevHiveContext _context;

		public FriendshipRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}
	}
}
