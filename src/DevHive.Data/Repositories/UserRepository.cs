using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevHive.Data.Repositories
{
	public class UserRepository : BaseRepository, IUserRepository
	{
		private readonly DevHiveContext _context;

		public UserRepository(DevHiveContext context)
		{
			this._context = context;
		}

		#region Create

		public async Task<bool> AddAsync(User entity)
		{
			await this._context.Users
				.AddAsync(entity);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> AddFriendToUserAsync(User user, User friend)
		{
			this._context.Update(user);
			user.Friends.Add(friend);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> AddLanguageToUserAsync(User user, Language language)
		{
			this._context.Update(user);

			user.Languages.Add(language);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> AddTechnologyToUserAsync(User user, Technology technology)
		{
			this._context.Update(user);

			user.Technologies.Add(technology);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read

		public IEnumerable<User> QueryAll()
		{
			return this._context.Users
				.Include(x => x.Roles)
				.AsNoTracking()
				.AsEnumerable();
		}

		public async Task<User> GetByIdAsync(Guid id)
		{
			return await this._context.Users
				.Include(x => x.Friends)
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<User> GetByUsernameAsync(string username)
		{
			return await this._context.Users
				.AsNoTracking()
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.FirstOrDefaultAsync(x => x.UserName == username);
		}

		public HashSet<Language> GetUserLanguages(User user)
		{
			return user.Languages;
		}

		public Language GetUserLanguage(User user, Language language)
		{
			return user.Languages
				.FirstOrDefault(x => x.Id == language.Id);
		}

		public HashSet<Technology> GetUserTechnologies(User user)
		{
			return user.Technologies;
		}

		public Technology GetUserTechnology(User user, Technology technology)
		{
			return user.Technologies
				.FirstOrDefault(x => x.Id == technology.Id);
		}
		#endregion

		#region Update

		public async Task<bool> EditAsync(User entity)
		{
			User user = await this._context.Users
				.FirstOrDefaultAsync(x => x.Id == entity.Id);

			this._context.Update(user);
			this._context.Entry(entity).CurrentValues.SetValues(entity);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> EditUserLanguageAsync(User user, Language oldLang, Language newLang)
		{
			this._context.Update(user);

			user.Languages.Remove(oldLang);
			user.Languages.Add(newLang);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> EditUserTechnologyAsync(User user, Technology oldTech, Technology newTech)
		{
			this._context.Update(user);

			user.Technologies.Remove(oldTech);
			user.Technologies.Add(newTech);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete

		public async Task<bool> DeleteAsync(User entity)
		{
			this._context.Users
				.Remove(entity);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> RemoveFriendAsync(User user, User friend)
		{
			this._context.Update(user);
			user.Friends.Remove(friend);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> RemoveLanguageFromUserAsync(User user, Language language)
		{
			this._context.Update(user);

			user.Languages.Remove(language);

			return await this.SaveChangesAsync(this._context);
		}

		public async Task<bool> RemoveTechnologyFromUserAsync(User user, Technology technology)
		{
			this._context.Update(user);

			user.Technologies.Remove(technology);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Validations

		public async Task<bool> DoesUserExistAsync(Guid id)
		{
			return await this._context.Users
				.AsNoTracking()
				.AnyAsync(x => x.Id == id);
		}

		public async Task<bool> DoesUsernameExistAsync(string username)
		{
			return await this._context.Users
				.AsNoTracking()
				.AnyAsync(u => u.UserName == username);
		}

		public async Task<bool> DoesEmailExistAsync(string email)
		{
			return await this._context.Users
				.AsNoTracking()
				.AnyAsync(u => u.Email == email);
		}

		public async Task<bool> DoesUserHaveThisFriendAsync(Guid userId, Guid friendId)
		{
			User user = await this._context.Users
				.FirstOrDefaultAsync(x => x.Id == userId);

			User friend = await this._context.Users
				.FirstOrDefaultAsync(x => x.Id == friendId);

			return user.Friends.Contains(friend);
		}

		public bool DoesUserHaveThisUsername(Guid id, string username)
		{
			return this._context.Users
				.AsNoTracking()
				.Any(x => x.Id == id &&
					x.UserName == username);
		}

		public bool DoesUserHaveFriends(User user)
		{
			return user.Friends.Count >= 1;
		}

		public bool DoesUserHaveThisLanguage(User user, Language language)
		{
			return user.Languages.Contains(language);
		}

		public bool DoesUserHaveThisTechnology(User user, Technology technology)
		{
			return user.Technologies.Contains(technology);
		}
		#endregion
	}
}
