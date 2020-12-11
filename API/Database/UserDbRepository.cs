using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Interfaces.Database;
using Microsoft.EntityFrameworkCore;
using Models.Classes;

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

		public async Task<bool> DoesUsernameExist(string username)
		{
			return await this._dbRepository.DbSet
				.SingleAsync(x => x.UserName == username) == null;
		}

		public async Task<bool> DoesUserExist(int id)
		{
			return await this._dbRepository.DbSet
				.SingleAsync(x => x.Id == id) == null;
		}
	}
}