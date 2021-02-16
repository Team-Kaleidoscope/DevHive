using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		Task<bool> AddRoleToUser(User user, string roleName);

		Task<User> GetByUsernameAsync(string username);
		Task<bool> UpdateProfilePicture(Guid userId, string pictureUrl);

		Task<bool> VerifyPassword(User user, string password);
		Task<bool> IsInRoleAsync(User user, string roleName);
		Task<bool> ValidateFriendsCollectionAsync(List<string> usernames);
		Task<bool> DoesEmailExistAsync(string email);
		Task<bool> DoesUserExistAsync(Guid id);
		Task<bool> DoesUsernameExistAsync(string username);
		Task<bool> DoesUserHaveThisUsernameAsync(Guid id, string username);
	}
}
