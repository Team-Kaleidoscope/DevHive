using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using Data.Models.Classes;

namespace API.Database
{
	public class UserDbRepository : DbRepository<User>
	{
		private readonly DbRepository<User> _dbRepository;

		public UserDbRepository(DbContext context)
			: base (context)
		{
			this._dbRepository = new DbRepository<User>(context);
		}

		public bool DoesUsernameExist(string username)
		{
			return this._dbRepository.DbSet
				.Any(x => x.UserName == username);
		}

		public bool DoesUserExist(int id)
		{
			return this._dbRepository.DbSet
				.Any(x => x.Id == id);
		}
	
		public bool HasThisUsername(int id, string username)
		{
			return this._dbRepository.DbSet
				.Any(x => x.Id == id &&
					x.UserName == username);
		}
	}
}
