using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<Role> _roleManager;

		public UserRepository(DevHiveContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
			: base(context)
		{
			this._userManager = userManager;
			this._roleManager = roleManager;
		}

		#region Create
		public override async Task<bool> AddAsync(User entity)
		{
			entity.PasswordHash = this._userManager.PasswordHasher.HashPassword(entity, entity.PasswordHash).ToString();
			IdentityResult result = await this._userManager.CreateAsync(entity);

			return result.Succeeded;
		}

		public async Task<bool> AddRoleToUser(User user, string roleName)
		{
			bool succeeded = (await this._userManager.AddToRoleAsync(user, roleName)).Succeeded;
			if (succeeded)
			{
				user.Roles.Add(await this._roleManager.FindByNameAsync(roleName));
				succeeded = await this.SaveChangesAsync();
			}

			return succeeded;
		}
		#endregion

		#region Read
		public override async Task<User> GetByIdAsync(Guid id)
		{
			return await this._userManager.Users
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
			return await this._userManager.Users
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
		public override async Task<bool> EditAsync(Guid id, User newEntity)
		{
			newEntity.Id = id;
			IdentityResult result = await this._userManager.UpdateAsync(newEntity);

			return result.Succeeded;
		}

		public async Task<bool> UpdateProfilePicture(Guid userId, string pictureUrl)
		{
			User user = await this.GetByIdAsync(userId);

			user.ProfilePicture.PictureURL = pictureUrl;

			return await this.SaveChangesAsync();
		}
		#endregion

		#region Delete
		public override async Task<bool> DeleteAsync(User entity)
		{
			IdentityResult result = await this._userManager.DeleteAsync(entity);

			return result.Succeeded;
		}
		#endregion

		#region Validations
		public async Task<bool> VerifyPassword(User user, string password)
		{
			return await this._userManager.CheckPasswordAsync(user, password);
		}

		public async Task<bool> IsInRoleAsync(User user, string roleName)
		{
			return await this._userManager.IsInRoleAsync(user, roleName);
		}

		public async Task<bool> DoesUserExistAsync(Guid id)
		{
			return await this._userManager.Users.AnyAsync(x => x.Id == id);
		}

		public async Task<bool> DoesUsernameExistAsync(string username)
		{
			return await this._userManager.Users
				.AsNoTracking()
				.AnyAsync(u => u.UserName == username);
		}

		public async Task<bool> DoesEmailExistAsync(string email)
		{
			return await this._userManager.Users
				.AsNoTracking()
				.AnyAsync(u => u.Email == email);
		}

		public async Task<bool> ValidateFriendsCollectionAsync(List<string> usernames)
		{
			bool valid = true;

			foreach (var username in usernames)
			{
				if (!await this.DoesUsernameExistAsync(username))
				{
					valid = false;
					break;
				}
			}
			return valid;
		}

		public async Task<bool> DoesUserHaveThisUsernameAsync(Guid id, string username)
		{
			return await this._userManager.Users
				.AnyAsync(x => x.Id == id &&
					x.UserName == username);
		}
		#endregion
	}
}
