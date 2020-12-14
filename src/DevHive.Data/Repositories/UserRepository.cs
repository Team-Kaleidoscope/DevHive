using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Data.Models;

namespace DevHive.Data.Repositories
{
	public class UserRepository : IRepository<User>
	{
		/* private readonly UserRepository<User> _dbRepository;

		public UserRepository(DbContext context)
			: base (context)
		{
			this._dbRepository = new DbRepository<User>(context);
		}

		public User FindByUsername(string username)
		{
			return this._dbRepository.DbSet
				.FirstOrDefault(usr => usr.UserName == username);
		}

		public bool DoesUsernameExist(string username)
		{
			return this._dbRepository.DbSet
				.Any(x => x.UserName == username);
		}

		public bool DoesUserExist(Guid id)
		{
			return this._dbRepository.DbSet
				.Any(x => x.Id == id);
		}
	
		public bool HasThisUsername(Guid id, string username)
		{
			return this._dbRepository.DbSet
				.Any(x => x.Id == id &&
					x.UserName == username);
		} */

		public Task AddAsync(User entity)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<User> Query(int count)
		{
			throw new System.NotImplementedException();
		}

		public Task<User> FindByIdAsync(object id)
		{
			throw new System.NotImplementedException();
		}

		public Task EditAsync(object id, User newEntity)
		{
			throw new System.NotImplementedException();
		}

		public Task DeleteAsync(object id)
		{
			throw new System.NotImplementedException();
		}
	}
}
