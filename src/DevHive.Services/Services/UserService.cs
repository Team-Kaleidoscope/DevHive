using AutoMapper;
using DevHive.Services.Options;
using DevHive.Services.Models.Identity.User;
using System.Threading.Tasks;
using DevHive.Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using DevHive.Common.Models.Identity;
using DevHive.Services.Interfaces;
using DevHive.Data.Interfaces.Repositories;
using System.Linq;
using DevHive.Common.Models.Misc;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;
using DevHive.Services.Models.Identity.Role;

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

			if (user.PasswordHash != PasswordModifications.GeneratePasswordHash(loginModel.Password))
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
			user.PasswordHash = PasswordModifications.GeneratePasswordHash(registerModel.Password);

			// Make sure the default role exists
			if (!await this._roleRepository.DoesNameExist(Role.DefaultRole))
				await this._roleRepository.AddAsync(new Role { Name = Role.DefaultRole });

			// Set the default role to the user
			Role defaultRole = await this._roleRepository.GetByNameAsync(Role.DefaultRole);
			user.Roles = new HashSet<Role>() { defaultRole };

			await this._userRepository.AddAsync(user);

			return new TokenModel(WriteJWTSecurityToken(user.Id, user.Roles));
		}
		#endregion

		#region Read
		public async Task<UserServiceModel> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id)
				?? throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(user);
		}

		public async Task<UserServiceModel> GetUserByUsername(string username)
		{
			User friend = await this._userRepository.GetByUsernameAsync(username);

			if (friend == null)
				throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(friend);
		}
		#endregion

		#region Update
		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateUserServiceModel)
		{
			await this.ValidateUserOnUpdate(updateUserServiceModel);

			await this.ValidateUserCollections(updateUserServiceModel);

			/* Roles */
			int roleCount = updateUserServiceModel.Roles.Count;
			for (int i = 0; i < roleCount; i++)
			{
				Role role = await this._roleRepository.GetByNameAsync(updateUserServiceModel.Roles.ElementAt(i).Name) ??
					throw new ArgumentException("Invalid role name!");

				UpdateRoleServiceModel updateRoleServiceModel = this._userMapper.Map<UpdateRoleServiceModel>(role);

				updateUserServiceModel.Roles.Add(updateRoleServiceModel);
			}

			/* Languages */
			int langCount = updateUserServiceModel.Languages.Count;
			for (int i = 0; i < langCount; i++)
			{
				Language language = await this._languageRepository.GetByNameAsync(updateUserServiceModel.Languages.ElementAt(i).Name) ??
					throw new ArgumentException("Invalid language name!");

				UpdateLanguageServiceModel updateLanguageServiceModel = this._userMapper.Map<UpdateLanguageServiceModel>(language);

				updateUserServiceModel.Languages.Add(updateLanguageServiceModel);
			}
			//Clean the already replaced languages
			updateUserServiceModel.Languages.RemoveWhere(x => x.Id == Guid.Empty);

			/* Technologies */
			int techCount = updateUserServiceModel.Technologies.Count;
			for (int i = 0; i < techCount; i++)
			{
				Technology technology = await this._technologyRepository.GetByNameAsync(updateUserServiceModel.Technologies.ElementAt(i).Name) ??
					throw new ArgumentException("Invalid technology name!");

				UpdateTechnologyServiceModel updateTechnologyServiceModel = this._userMapper.Map<UpdateTechnologyServiceModel>(technology);

				updateUserServiceModel.Technologies.Add(updateTechnologyServiceModel);
			}
			//Clean the already replaced technologies
			updateUserServiceModel.Technologies.RemoveWhere(x => x.Id == Guid.Empty);

			/* Friends */
			HashSet<User> friends = new();
			int friendsCount = updateUserServiceModel.Friends.Count;
			for (int i = 0; i < friendsCount; i++)
			{
				User friend = await this._userRepository.GetByUsernameAsync(updateUserServiceModel.Friends.ElementAt(i).Name) ??
					throw new ArgumentException("Invalid friend's username!");

				friends.Add(friend);
			}
			//Clean the already replaced technologies
			updateUserServiceModel.Friends.RemoveWhere(x => x.Id == Guid.Empty);

			User user = this._userMapper.Map<User>(updateUserServiceModel);
			user.Friends = friends;

			bool successful = await this._userRepository.EditAsync(updateUserServiceModel.Id, user);

			if (!successful)
				throw new InvalidOperationException("Unable to edit user!");

			return this._userMapper.Map<UserServiceModel>(user);
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
		#endregion

		#region Validations
		public async Task<bool> ValidJWT(Guid id, string rawTokenData)
		{
			// There is authorization name in the beginning, i.e. "Bearer eyJh..."
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			Guid jwtUserID = new Guid(this.GetClaimTypeValues("ID", jwt.Claims).First());
			List<string> jwtRoleNames = this.GetClaimTypeValues("role", jwt.Claims);

			User user = await this._userRepository.GetByIdAsync(jwtUserID)
				?? throw new ArgumentException("User does not exist!");

			/* Check if user is trying to do something to himself, unless he's an admin */

			/* Check roles */
			if (jwtRoleNames.Contains(Role.AdminRole))
				return true;

			if (!jwtRoleNames.Contains(Role.AdminRole))
				if (user.Id != id)
					return false;

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

		private async Task ValidateUserOnUpdate(UpdateUserServiceModel updateUserServiceModel)
		{
			if (!await this._userRepository.DoesUserExistAsync(updateUserServiceModel.Id))
				throw new ArgumentException("User does not exist!");

			if (!this._userRepository.DoesUserHaveThisUsername(updateUserServiceModel.Id, updateUserServiceModel.UserName)
					&& await this._userRepository.DoesUsernameExistAsync(updateUserServiceModel.UserName))
				throw new ArgumentException("Username already exists!");
		}

		private async Task ValidateUserCollections(UpdateUserServiceModel updateUserServiceModel)
		{
			// Friends
			foreach (var friend in updateUserServiceModel.Friends)
			{
				User returnedFriend = await this._userRepository.GetByUsernameAsync(friend.Name);

				if (returnedFriend == null)
					throw new ArgumentException($"User {friend.Name} does not exist!");
			}

			// Languages
			foreach (var language in updateUserServiceModel.Languages)
			{
				Language returnedLanguage = await this._languageRepository.GetByNameAsync(language.Name);

				if (returnedLanguage == null)
					throw new ArgumentException($"Language {language.Name} does not exist!");
			}

			// Technology
			foreach (var technology in updateUserServiceModel.Technologies)
			{
				Technology returnedTechnology = await this._technologyRepository.GetByNameAsync(technology.Name);

				if (returnedTechnology == null)
					throw new ArgumentException($"Technology {technology.Name} does not exist!");
			}
		}

		private string WriteJWTSecurityToken(Guid userId, HashSet<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			HashSet<Claim> claims = new()
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
		#endregion

		public async Task<Guid> SuperSecretPromotionToAdmin(Guid userId)
		{
			User user = await this._userRepository.GetByIdAsync(userId) ??
				throw new ArgumentException("User does not exist! Can't promote shit in this country...");

			if(!await this._roleRepository.DoesNameExist("Admin"))
			{
				Role adminRole = new()
				{
					Name = Role.AdminRole
				};
				adminRole.Users.Add(user);

				await this._roleRepository.AddAsync(adminRole);
			}

			Role admin = await this._roleRepository.GetByNameAsync(Role.AdminRole);

			user.Roles.Add(admin);
			await this._userRepository.EditAsync(user.Id, user);

			return admin.Id;
		}
	}
}
