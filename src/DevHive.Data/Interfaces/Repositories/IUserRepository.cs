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
		Language GetUserLanguage(User user, Language language);
		HashSet<Language> GetUserLanguages(User user);
		HashSet<Technology> GetUserTechnologies(User user);
		Technology GetUserTechnology(User user, Technology technology);
		IEnumerable<User> QueryAll();

		//Validations
		Task<bool> DoesEmailExistAsync(string email);
		Task<bool> DoesUserExistAsync(Guid id);
		Task<bool> DoesUserHaveThisFriendAsync(Guid userId, Guid friendId);
		Task<bool> DoesUsernameExistAsync(string username);
		bool DoesUserHaveThisLanguage(User user, Language language);
		bool DoesUserHaveThisUsername(Guid id, string username);
		bool DoesUserHaveFriends(User user);
		bool DoesUserHaveThisTechnology(User user, Technology technology);
	}
}
