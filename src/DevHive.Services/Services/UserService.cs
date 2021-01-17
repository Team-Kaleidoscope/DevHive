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
using DevHive.Services.Interfaces;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ILanguageRepository _languageRepository;
		private readonly ITechnologyRepository _technologyRepository;
		private readonly IMapper _userMapper;
		private readonly JWTOptions _jwtOptions;

		public UserService(IUserRepository userRepository,
			ILanguageRepository languageRepository,
			IRoleRepository roleRepository,
			ITechnologyRepository technologyRepository,
			IMapper mapper,
			JWTOptions jwtOptions)
		{
			this._userRepository = userRepository;
			this._roleRepository = roleRepository;
			this._userMapper = mapper;
			this._jwtOptions = jwtOptions;
			this._languageRepository = languageRepository;
			this._technologyRepository = technologyRepository;
		}

		#region Authentication

		public async Task<TokenModel> LoginUser(LoginServiceModel loginModel)
		{
			if (!await this._userRepository.DoesUsernameExistAsync(loginModel.UserName))
				throw new ArgumentException("Invalid username!");

			User user = await this._userRepository.GetByUsernameAsync(loginModel.UserName);

			if (user.PasswordHash != GeneratePasswordHash(loginModel.Password))
				throw new ArgumentException("Incorrect password!");

			return new TokenModel(WriteJWTSecurityToken(user.Id, user.Roles));
		}

		public async Task<TokenModel> RegisterUser(RegisterServiceModel registerModel)
		{
			if (await this._userRepository.DoesUsernameExistAsync(registerModel.UserName))
				throw new ArgumentException("Username already exists!");

			if (await this._userRepository.DoesEmailExistAsync(registerModel.Email))
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

			return new TokenModel(WriteJWTSecurityToken(user.Id, user.Roles));
		}
		#endregion

		#region Create

		public async Task<bool> AddFriend(Guid userId, Guid friendId)
		{
			Task<bool> userExists = this._userRepository.DoesUserExistAsync(userId);
			Task<bool> friendExists = this._userRepository.DoesUserExistAsync(friendId);

			await Task.WhenAll(userExists, friendExists);

			if (!userExists.Result)
				throw new ArgumentException("User doesn't exist!");

			if (!friendExists.Result)
				throw new ArgumentException("Friend doesn't exist!");

			if (await this._userRepository.DoesUserHaveThisFriendAsync(userId, friendId))
				throw new ArgumentException("Friend already exists in your friends list.");

			User user = await this._userRepository.GetByIdAsync(userId);
			User friend = await this._userRepository.GetByIdAsync(friendId);

			return user != default(User) && friend != default(User) ?
				await this._userRepository.AddFriendToUserAsync(user, friend) :
				throw new ArgumentException("Invalid user!");
		}
		#endregion

		#region Read

		public async Task<UserServiceModel> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id)
				?? throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(user);
		}

		public async Task<UserServiceModel> GetFriend(string username)
		{
			User friend = await this._userRepository.GetByUsernameAsync(username);

			if (default(User) == friend)
				throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(friend);
		}
		#endregion

		#region Update

		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateUserServiceModel)
		{
			if (!await this._userRepository.DoesUserExistAsync(updateUserServiceModel.Id))
				throw new ArgumentException("User does not exist!");

			if (!this._userRepository.DoesUserHaveThisUsername(updateUserServiceModel.Id, updateUserServiceModel.UserName)
					&& await this._userRepository.DoesUsernameExistAsync(updateUserServiceModel.UserName))
				throw new ArgumentException("Username already exists!");

			await this.ValidateUserCollections(updateUserServiceModel);

			//Query proper lang, tech and role and insert the full class in updateUserServiceModel
			List<Language> properLanguages = new();
			foreach (UpdateUserCollectionServiceModel lang in updateUserServiceModel.Languages)
				properLanguages.Add(await this._languageRepository.GetByNameAsync(lang.Name));

			User user = this._userMapper.Map<User>(updateUserServiceModel);
			user.Languages = properLanguages;

			bool success = await this._userRepository.EditAsync(user);

			if (!success)
				throw new InvalidOperationException("Unable to edit user!");

			return this._userMapper.Map<UserServiceModel>(user); ;
		}

		private async Task ValidateUserCollections(UpdateUserServiceModel updateUserServiceModel)
		{
			// Friends
			foreach (UpdateUserCollectionServiceModel friend in updateUserServiceModel.Friends)
			{
				User returnedFriend = await this._userRepository.GetByUsernameAsync(friend.Name);

				if (default(User) == returnedFriend)
					throw new ArgumentException($"User {friend.Name} does not exist!");
			}

			// Languages
			foreach (UpdateUserCollectionServiceModel language in updateUserServiceModel.Languages)
			{
				Language returnedLanguage = await this._languageRepository.GetByNameAsync(language.Name);

				if (default(Language) == returnedLanguage)
					throw new ArgumentException($"Language {language.Name} does not exist!");
			}

			// Technology
			foreach (UpdateUserCollectionServiceModel technology in updateUserServiceModel.Technologies)
			{
				Technology returnedTechnology = await this._technologyRepository.GetByNameAsync(technology.Name);

				if (default(Technology) == returnedTechnology)
					throw new ArgumentException($"Technology {technology.Name} does not exist!");
			}
		}
		#endregion

		#region Delete

		public async Task DeleteUser(Guid id)
		{
			if (!await this._userRepository.DoesUserExistAsync(id))
				throw new ArgumentException("User does not exist!");

			User user = await this._userRepository.GetByIdAsync(id);
			bool result = await this._userRepository.DeleteAsync(user);

			if (!result)
				throw new InvalidOperationException("Unable to delete user!");
		}

		public async Task<bool> RemoveFriend(Guid userId, Guid friendId)
		{
			bool userExists = await this._userRepository.DoesUserExistAsync(userId);
			bool friendExists = await this._userRepository.DoesUserExistAsync(friendId);

			if (!userExists)
				throw new ArgumentException("User doesn't exist!");

			if (!friendExists)
				throw new ArgumentException("Friend doesn't exist!");

			if (!await this._userRepository.DoesUserHaveThisFriendAsync(userId, friendId))
				throw new ArgumentException("This ain't your friend, amigo.");

			User user = await this._userRepository.GetByIdAsync(userId);
			User homie = await this._userRepository.GetByIdAsync(friendId);

			return await this._userRepository.RemoveFriendAsync(user, homie);
		}
		#endregion

		#region Validations

		public async Task<bool> ValidJWT(Guid id, string rawTokenData)
		{
			// There is authorization name in the beginning, i.e. "Bearer eyJh..."
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			Guid jwtUserID = new Guid(this.GetClaimTypeValues("ID", jwt.Claims)[0]);
			List<string> jwtRoleNames = this.GetClaimTypeValues("role", jwt.Claims);

			User user = await this._userRepository.GetByIdAsync(jwtUserID)
				?? throw new ArgumentException("User does not exist!");

			/* Check if user is trying to do something to himself, unless he's an admin */

			if (!jwtRoleNames.Contains(Role.AdminRole))
				if (user.Id != id)
					return false;

			/* Check roles */

			// Check if jwt contains all user roles (if it doesn't, jwt is either old or tampered with)
			foreach (var role in user.Roles)
			{
				if (!jwtRoleNames.Contains(role.Name))
					return false;
			}

			// Check if jwt contains only roles of user
			if (jwtRoleNames.Count != user.Roles.Count)
				return false;

			return true;
		}

		private List<string> GetClaimTypeValues(string type, IEnumerable<Claim> claims)
		{
			List<string> toReturn = new();

			foreach (var claim in claims)
				if (claim.Type == type)
					toReturn.Add(claim.Value);

			return toReturn;
		}

		private string WriteJWTSecurityToken(Guid userId, IList<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			List<Claim> claims = new()
			{
				new Claim("ID", $"{userId}"),
			};

			foreach (var role in roles)
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

		private string GeneratePasswordHash(string password)
		{
			return string.Join(string.Empty, SHA512.HashData(Encoding.ASCII.GetBytes(password)));
		}
		#endregion
	}
}
