using DevHive.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevHive.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
	{
		public Task<bool> AddFriendAsync(User user, User friend);

		public IEnumerable<User> QueryAll();

		public Task<User> GetByUsername(string username);

		public Task<bool> RemoveFriendAsync(User user, User friend);

		public bool DoesUserExist(Guid id);

		public bool DoesUserHaveThisUsername(Guid id, string username);

		public Task<bool> DoesUsernameExist(string username);

		public Task<bool> DoesEmailExist(string email);
	}
}
