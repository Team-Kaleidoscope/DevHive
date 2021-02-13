using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Mappers;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Data.Models.Relational;
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
		public override async Task<User> GetByIdAsync(Guid id)
		{
			return await this._context.Users
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.Include(x => x.Posts)
				.Include(x => x.Friends)
				.Include(x => x.ProfilePicture)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<User> GetByUsernameAsync(string username)
		{
			return await this._context.Users
				.Include(x => x.Roles)
				.Include(x => x.Languages)
				.Include(x => x.Technologies)
				.Include(x => x.Posts)
				.Include(x => x.Friends)
				.Include(x => x.ProfilePicture)
				.FirstOrDefaultAsync(x => x.UserName == username);
		}
		#endregion

		#region Update
		public async Task<bool> UpdateProfilePicture(Guid userId, string pictureUrl)
		{
			User user = await this.GetByIdAsync(userId);

			user.ProfilePicture.PictureURL = pictureUrl;

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

		public async Task<bool> ValidateFriendsCollectionAsync(List<string> usernames)
		{
			bool valid = true;

			foreach (var username in usernames)
			{
				if (!await this._context.Users.AnyAsync(x => x.UserName == username))
				{
					valid = false;
					break;
				}
			}
			return valid;
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
