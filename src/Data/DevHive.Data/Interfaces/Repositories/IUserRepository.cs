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
		Task<bool> UpdateProfilePicture(Guid userId, string pictureUrl);

		//Validations
		Task<bool> ValidateFriendsCollectionAsync(List<string> usernames);
		Task<bool> DoesEmailExistAsync(string email);
		Task<bool> DoesUserExistAsync(Guid id);
		Task<bool> DoesUsernameExistAsync(string username);
		bool DoesUserHaveThisUsername(Guid id, string username);
	}
}
