using AutoMapper;
using DevHive.Services.Options;
using DevHive.Services.Models.Identity.User;
using System.Threading.Tasks;
using DevHive.Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using DevHive.Common.Models.Identity;
using DevHive.Data.Repositories.Contracts;

namespace DevHive.Services.Services
{
	public class UserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly IMapper _userMapper;
		private readonly JWTOptions _jwtOptions;

		public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, JWTOptions jwtOptions)
		{
			this._userRepository = userRepository;
			this._roleRepository = roleRepository;
			this._userMapper = mapper;
			this._jwtOptions = jwtOptions;
		}

		public async Task<TokenModel> LoginUser(LoginServiceModel loginModel)
		{
			if (!await this._userRepository.DoesUsernameExist(loginModel.UserName))
				throw new ArgumentException("Invalid username!");

			User user = await this._userRepository.GetByUsername(loginModel.UserName);

			if (user.PasswordHash != GeneratePasswordHash(loginModel.Password))
				throw new ArgumentException("Incorrect password!");

			return new TokenModel(WriteJWTSecurityToken(user.UserName, user.Roles));
		}

		public async Task<TokenModel> RegisterUser(RegisterServiceModel registerModel)
		{
			if (await this._userRepository.DoesUsernameExist(registerModel.UserName))
				throw new ArgumentException("Username already exists!");

			if (await this._userRepository.DoesEmailExist(registerModel.Email))
				throw new ArgumentException("Email already exists!");

			User user = this._userMapper.Map<User>(registerModel);
			user.PasswordHash = GeneratePasswordHash(registerModel.Password);

			// Make sure the default role exists
			if (!await this._roleRepository.DoesNameExist(Role.DefaultRole))
				await this._roleRepository.AddAsync(new Role { Name = Role.DefaultRole });

			// Set the default role to the user
			Role defaultRole = await this._roleRepository.GetByNameAsync(Role.DefaultRole);
			user.Roles = new List<Role>() { defaultRole };

			await this._userRepository.AddAsync(user);

			return new TokenModel(WriteJWTSecurityToken(user.UserName, user.Roles));
		}

		public async Task<bool> AddFriend(Guid userId, Guid friendId)
		{
			User user = await this._userRepository.GetByIdAsync(userId);
			User friend = await this._userRepository.GetByIdAsync(friendId);

			if (DoesUserHaveThisFriend(user, friend))
				throw new ArgumentException("Friend already exists in your friends list.");

			return user != default(User) && friend != default(User) ? 
				await this._userRepository.AddFriendAsync(user, friend) : 
				throw new ArgumentException("Invalid user!");
		}

		public async Task<UserServiceModel> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id)
				?? throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(user);
		}

		public async Task<UserServiceModel> GetFriendById(Guid friendId)
		{
			if(!_userRepository.DoesUserExist(friendId))
				throw new ArgumentException("User does not exist!");

			User friend = await this._userRepository.GetByIdAsync(friendId);

			return this._userMapper.Map<UserServiceModel>(friend);
		}

		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateModel)
		{
			if (!this._userRepository.DoesUserExist(updateModel.Id))
				throw new ArgumentException("User does not exist!");

			if (!this._userRepository.DoesUserHaveThisUsername(updateModel.Id, updateModel.UserName)
					&& await this._userRepository.DoesUsernameExist(updateModel.UserName))
				throw new ArgumentException("Username already exists!");

			User user = this._userMapper.Map<User>(updateModel);
			bool result = await this._userRepository.EditAsync(user);

			if (!result)
				throw new InvalidOperationException("Unable to edit user!");

			return this._userMapper.Map<UserServiceModel>(user);;
		}

		public async Task DeleteUser(Guid id)
		{
			if (!this._userRepository.DoesUserExist(id))
				throw new ArgumentException("User does not exist!");

			User user = await this._userRepository.GetByIdAsync(id);
			bool result = await this._userRepository.DeleteAsync(user);

			if (!result)
				throw new InvalidOperationException("Unable to delete user!");
		}

		public async Task<bool> RemoveFriend(Guid userId, Guid friendId)
		{
			if(!this._userRepository.DoesUserExist(userId) && 
				!this._userRepository.DoesUserExist(friendId))
					throw new ArgumentException("Invalid user!");

			User user = await this._userRepository.GetByIdAsync(userId);
			User friend = await this._userRepository.GetByIdAsync(friendId);

			if(!this.DoesUserHaveFriends(user))
				throw new ArgumentException("User does not have any friends.");

			if (!DoesUserHaveThisFriend(user, friend))
				throw new ArgumentException("This ain't your friend, amigo.");

			return await this.RemoveFriend(user.Id, friendId);
		}

		public async Task<bool> ValidJWT(Guid id, string rawTokenData)
		{
			// There is authorization name in the beginning, i.e. "Bearer eyJh..."
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			string jwtUserName = this.GetClaimTypeValues("unique_name", jwt.Claims)[0];
			List<string> jwtRoleNames = this.GetClaimTypeValues("role", jwt.Claims);
			
			User user = await this._userRepository.GetByUsername(jwtUserName)
				?? throw new ArgumentException("User does not exist!");

			/* Username check, only when user isn't admin */

			if (!jwtRoleNames.Contains(Role.AdminRole))
				if (!this._userRepository.DoesUserHaveThisUsername(id, jwtUserName))
					return false;

			/* Check roles */
			
			// Check if jwt contains all user roles (if it doesn't, jwt is either old or tampered with)
			foreach(var role in user.Roles)
			{
				if (!jwtRoleNames.Contains(role.Name))
					return false;
			}

			// Check if jwt contains only roles of user
			if (jwtRoleNames.Count != user.Roles.Count)
				return false;

			return true;
		}

		private string GeneratePasswordHash(string password)
		{
			return string.Join(string.Empty, SHA512.HashData(Encoding.ASCII.GetBytes(password)));
		}

		private bool DoesUserHaveThisFriend(User user, User friend)
		{
			return user.Friends.Contains(friend);
		}

		private bool DoesUserHaveFriends(User user)
		{
			return user.Friends.Count >= 1;
		}

		private List<string> GetClaimTypeValues(string type, IEnumerable<Claim> claims)
		{
			List<string> toReturn = new();
			
			foreach(var claim in claims)
				if (claim.Type == type)
					toReturn.Add(claim.Value);

			return toReturn;
		}

		private string WriteJWTSecurityToken(string userName, IList<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, userName),
			};

			foreach(var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Today.AddDays(7),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(signingKey),
					SecurityAlgorithms.HmacSha512Signature)
			};

			JwtSecurityTokenHandler tokenHandler = new();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
