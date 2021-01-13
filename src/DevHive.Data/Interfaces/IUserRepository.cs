using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		Task<bool> AddFriendAsync(User user, User friend);
		Task<bool> AddLanguageToUserAsync(User user, Language language);
		Task<bool> AddTechnologyToUserAsync(User user, Technology technology);
		
		Task<User> GetByUsername(string username);
		Language GetUserLanguage(User user, Language language);
		IList<Language> GetUserLanguages(User user);
		IList<Technology> GetUserTechnologies(User user);
		Technology GetUserTechnology(User user, Technology technology);
		IEnumerable<User> QueryAll();

		Task<bool> EditUserLanguage(User user, Language oldLang, Language newLang);
		Task<bool> EditUserTechnologies(User user, Technology oldTech, Technology newTech);

		Task<bool> RemoveFriendAsync(User user, User friend);
		Task<bool> RemoveLanguageFromUserAsync(User user, Language language);
		Task<bool> RemoveTechnologyFromUserAsync(User user, Technology technology);

		bool DoesUserHaveThisLanguage(User user, Language language);
		bool DoesUserHaveThisUsername(Guid id, string username);
		bool DoesUserHaveFriends(User user);
		Task<bool> DoesEmailExistAsync(string email);
		Task<bool> DoesUserExistAsync(Guid id);
		Task<bool> DoesUserHaveThisFriendAsync(Guid userId, Guid friendId);
		Task<bool> DoesUsernameExistAsync(string username);
	}
}