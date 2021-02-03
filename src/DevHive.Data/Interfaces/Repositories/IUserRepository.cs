using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface IUserRepository : IRepository<User>
	{
		//Read
		Task<User> GetByUsernameAsync(string username);
		IEnumerable<User> QueryAll();
		Task<bool> UpdateProfilePicture(Guid userId, string pictureUrl);

		//Validations
		Task<bool> DoesEmailExistAsync(string email);
		Task<bool> DoesUserExistAsync(Guid id);
		Task<bool> DoesUserHaveThisFriendAsync(Guid userId, Guid friendId);
		bool DoesUserHaveThisUsername(Guid id, string username);
		Task<bool> DoesUsernameExistAsync(string username);
	}
}
