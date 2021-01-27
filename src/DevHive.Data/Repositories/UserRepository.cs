using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
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
				.Include(x => x.Friends)
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<User> GetByUsernameAsync(string username)
		{
			return await this._context.Users
				.Include(x => x.Friends)
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.FirstOrDefaultAsync(x => x.UserName == username);
		}
		#endregion

		#region Update
		public override async Task<bool> EditAsync(Guid id, User newEntity)
		{
			User user = await this.GetByIdAsync(id);

			user.Languages.Clear();
			foreach (var lang in newEntity.Languages)
				user.Languages.Add(lang);

			user.Roles.Clear();
			foreach (var role in newEntity.Roles)
				user.Roles.Add(role);

			foreach (var friend in user.Friends)
			{
				friend.Friends.Remove(user);
				this._context.Entry(friend).State = EntityState.Modified;
			}
			user.Friends.Clear();
			foreach (var friend in newEntity.Friends)
			{
				friend.Friends.Add(user);
				user.Friends.Add(friend);
			}

			user.Technologies.Clear();
			foreach (var tech in newEntity.Technologies)
				user.Technologies.Add(tech);

			this._context.Entry(user).State = EntityState.Modified;

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
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == userId);

			User friend = await this._context.Users
				.AsNoTracking()
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
		#endregion
	}
}
