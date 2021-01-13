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
using DevHive.Services.Models.Language;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;
using DevHive.Data.Repositories;
using DevHive.Data.Interfaces;

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

			User user = await this._userRepository.GetByUsername(loginModel.UserName);

			if (user.PasswordHash != GeneratePasswordHash(loginModel.Password))
				throw new ArgumentException("Incorrect password!");

			return new TokenModel(WriteJWTSecurityToken(user.UserName, user.Roles));
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

			return new TokenModel(WriteJWTSecurityToken(user.UserName, user.Roles));
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
				await this._userRepository.AddFriendAsync(user, friend) :
				throw new ArgumentException("Invalid user!");
		}

		public async Task<bool> AddLanguageToUser(Guid userId, LanguageServiceModel languageServiceModel)
		{
			bool userExists = await this._userRepository.DoesUserExistAsync(userId);
			bool languageExists = await this._languageRepository.DoesLanguageExistAsync(languageServiceModel.Id);

			if (!userExists)
				throw new ArgumentException("User does not exist!");

			if (!languageExists)
				throw new ArgumentException("Language does noy exist!");

			User user = await this._userRepository.GetByIdAsync(userId);
			Language language = await this._languageRepository.GetByIdAsync(languageServiceModel.Id);

			if (this._userRepository.DoesUserHaveThisLanguage(user, language))
				throw new ArgumentException("User already has this language!");

			return await this._userRepository.AddLanguageToUserAsync(user, language);
		}

		public async Task<bool> AddTechnologyToUser(Guid userId, TechnologyServiceModel technologyServiceModel)
		{
			bool userExists = await this._userRepository.DoesUserExistAsync(userId);
			bool technologyExists = await this._technologyRepository.DoesTechnologyExistAsync(technologyServiceModel.Id);

			if (!userExists)
				throw new ArgumentException("User does not exist!");

			if (!technologyExists)
				throw new ArgumentException("Technology does not exist!");

			Technology technology = await this._technologyRepository.GetByIdAsync(technologyServiceModel.Id);
			User user = await this._userRepository.GetByIdAsync(userId);

			if (this._userRepository.DoesUserHaveThisTechnology(user, technology))
				throw new ArgumentException("User already has this language!");

			return await this._userRepository.AddTechnologyToUserAsync(user, technology);
		}
		#endregion

		#region Read

		public async Task<UserServiceModel> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id)
				?? throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(user);
		}

		public async Task<UserServiceModel> GetFriendById(Guid friendId)
		{
			if (!await _userRepository.DoesUserExistAsync(friendId))
				throw new ArgumentException("User does not exist!");

			User friend = await this._userRepository.GetByIdAsync(friendId);

			return this._userMapper.Map<UserServiceModel>(friend);
		}
		#endregion

		#region Update

		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateModel)
		{
			if (!await this._userRepository.DoesUserExistAsync(updateModel.Id))
				throw new ArgumentException("User does not exist!");

			if (!this._userRepository.DoesUserHaveThisUsername(updateModel.Id, updateModel.UserName)
					&& await this._userRepository.DoesUsernameExistAsync(updateModel.UserName))
				throw new ArgumentException("Username already exists!");

			User user = this._userMapper.Map<User>(updateModel);
			bool result = await this._userRepository.EditAsync(user);

			if (!result)
				throw new InvalidOperationException("Unable to edit user!");

			return this._userMapper.Map<UserServiceModel>(user); ;
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
			Task<bool> userExists = this._userRepository.DoesUserExistAsync(userId);
			Task<bool> friendExists = this._userRepository.DoesUserExistAsync(friendId);

			await Task.WhenAll(userExists, friendExists);

			if (!userExists.Result)
				throw new ArgumentException("User doesn't exist!");

			if (!friendExists.Result)
				throw new ArgumentException("Friend doesn't exist!");

			if (!await this._userRepository.DoesUserHaveThisFriendAsync(userId, friendId))
				throw new ArgumentException("This ain't your friend, amigo.");

			return await this.RemoveFriend(userId, friendId);
		}

		public async Task<bool> RemoveLanguageFromUser(Guid userId, LanguageServiceModel languageServiceModel)
		{
			bool userExists = await this._userRepository.DoesUserExistAsync(userId);
			bool languageExists = await this._languageRepository.DoesLanguageExistAsync(languageServiceModel.Id);

			if (!userExists)
				throw new ArgumentException("User does not exist!");

			if (!languageExists)
				throw new ArgumentException("Language does not exist!");

			User user = await this._userRepository.GetByIdAsync(userId);
			Language language = await this._languageRepository.GetByIdAsync(languageServiceModel.Id);

			if (!this._userRepository.DoesUserHaveThisLanguage(user, language))
				throw new ArgumentException("User does not have this language!");

			return await this._userRepository.RemoveLanguageFromUserAsync(user, language);
		}

		public async Task<bool> RemoveTechnologyFromUser(Guid userId, TechnologyServiceModel technologyServiceModel)
		{
			bool userExists = await this._userRepository.DoesUserExistAsync(userId);
			bool technologyExists = await this._technologyRepository.DoesTechnologyExistAsync(technologyServiceModel.Id);

			if (!userExists)
				throw new ArgumentException("User does not exist!");

			if (!technologyExists)
				throw new ArgumentException("Language does not exist!");

			User user = await this._userRepository.GetByIdAsync(userId);
			Technology technology = await this._technologyRepository.GetByIdAsync(technologyServiceModel.Id);

			if (!this._userRepository.DoesUserHaveThisTechnology(user, technology))
				throw new ArgumentException("User does not have this technology!");

			return await this._userRepository.RemoveTechnologyFromUserAsync(user, technology);
		}
		#endregion

		#region Validations

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

		private string WriteJWTSecurityToken(string userName, IList<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, userName),
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
