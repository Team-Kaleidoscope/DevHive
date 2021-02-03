using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Data.RelationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		private readonly DevHiveContext _context;

		public UserRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		#region Read
		public IEnumerable<User> QueryAll()
		{
			return this._context.Users
				.Include(x => x.Roles)
				.AsNoTracking()
				.AsEnumerable();
		}

		public override async Task<User> GetByIdAsync(Guid id)
		{
			return await this._context.Users
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.Include(x => x.Posts)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<User> GetByUsernameAsync(string username)
		{
			return await this._context.Users
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.Include(x => x.Posts)
				.FirstOrDefaultAsync(x => x.UserName == username);
		}
		#endregion

		#region Update
		public override async Task<bool> EditAsync(Guid id, User newEntity)
		{
			User user = await this.GetByIdAsync(id);

			this._context
				.Entry(user)
				.CurrentValues
				.SetValues(newEntity);

			HashSet<Language> languages = new();
			foreach (var lang in newEntity.Languages)
				languages.Add(lang);
			user.Languages = languages;

			HashSet<Role> roles = new();
			foreach (var role in newEntity.Roles)
				roles.Add(role);
			user.Roles = roles;

			foreach (var friend in newEntity.MyFriends)
			{
				user.MyFriends.Add(friend);
				this._context.Entry(friend).State = EntityState.Modified;
			}

			foreach (var friend in newEntity.FriendsOf)
			{
				user.FriendsOf.Add(friend);
				this._context.Entry(friend).State = EntityState.Modified;
			}

			HashSet<Technology> technologies = new();
			foreach (var tech in newEntity.Technologies)
				technologies.Add(tech);
			user.Technologies = technologies;

			this._context.Entry(user).State = EntityState.Modified;

			return await this.SaveChangesAsync();
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
			return true;
			// User user = await this.GetByIdAsync(userId);

			// User friend = await this.GetByIdAsync(friendId);

			// return user.Friends.Any(x => x.Friend.Id == friendId);
		}

		public bool DoesUserHaveThisUsername(Guid id, string username)
		{
			return this._context.Users
				.AsNoTracking()
				.Any(x => x.Id == id &&
					x.UserName == username);
		}
		#endregion
	}
}
