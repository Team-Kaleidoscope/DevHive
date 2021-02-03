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
using DevHive.Data.RelationModels;
using Microsoft.AspNetCore.Http;

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
		private readonly ICloudService _cloudService;

		public UserService(IUserRepository userRepository,
			ILanguageRepository languageRepository,
			IRoleRepository roleRepository,
			ITechnologyRepository technologyRepository,
			IMapper mapper,
			JWTOptions jwtOptions,
			ICloudService cloudService)
		{
			this._userRepository = userRepository;
			this._roleRepository = roleRepository;
			this._userMapper = mapper;
			this._jwtOptions = jwtOptions;
			this._languageRepository = languageRepository;
			this._technologyRepository = technologyRepository;
			this._cloudService = cloudService;
		}

		#region Authentication
		public async Task<TokenModel> LoginUser(LoginServiceModel loginModel)
		{
			if (!await this._userRepository.DoesUsernameExistAsync(loginModel.UserName))
				throw new ArgumentException("Invalid username!");

			User user = await this._userRepository.GetByUsernameAsync(loginModel.UserName);

			if (user.PasswordHash != PasswordModifications.GeneratePasswordHash(loginModel.Password))
				throw new ArgumentException("Incorrect password!");

			return new TokenModel(WriteJWTSecurityToken(user.Id, user.UserName, user.Roles));
		}

		public async Task<TokenModel> RegisterUser(RegisterServiceModel registerModel)
		{
			if (await this._userRepository.DoesUsernameExistAsync(registerModel.UserName))
				throw new ArgumentException("Username already exists!");

			if (await this._userRepository.DoesEmailExistAsync(registerModel.Email))
				throw new ArgumentException("Email already exists!");

			User user = this._userMapper.Map<User>(registerModel);
			user.PasswordHash = PasswordModifications.GeneratePasswordHash(registerModel.Password);
			user.ProfilePicture = new ProfilePicture() { PictureURL = String.Empty };

			// Make sure the default role exists
			//TODO: Move when project starts
			if (!await this._roleRepository.DoesNameExist(Role.DefaultRole))
				await this._roleRepository.AddAsync(new Role { Name = Role.DefaultRole });

			// Set the default role to the user
			Role defaultRole = await this._roleRepository.GetByNameAsync(Role.DefaultRole);
			user.Roles.Add(defaultRole);

			await this._userRepository.AddAsync(user);

			return new TokenModel(WriteJWTSecurityToken(user.Id, user.UserName, user.Roles));
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
			User user = await this._userRepository.GetByUsernameAsync(username);

			if (user == null)
				throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(user);
		}
		#endregion

		#region Update
		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateUserServiceModel)
		{
			await this.ValidateUserOnUpdate(updateUserServiceModel);

			User user = await this.PopulateModel(updateUserServiceModel);

			await this.CreateRelationToFriends(user, updateUserServiceModel.Friends.ToList());

			bool successful = await this._userRepository.EditAsync(updateUserServiceModel.Id, user);

			if (!successful)
				throw new InvalidOperationException("Unable to edit user!");

			return this._userMapper.Map<UserServiceModel>(
				await this._userRepository.GetByIdAsync(user.Id));
		}

		public async Task<ProfilePictureServiceModel> UpdateProfilePicture(UpdateProfilePictureServiceModel updateProfilePictureServiceModel)
		{
			User user = await this._userRepository.GetByIdAsync(updateProfilePictureServiceModel.UserId);

			if (!String.IsNullOrEmpty(user.ProfilePicture.PictureURL))
			{
				bool success = await _cloudService.RemoveFilesFromCloud(new List<string> { user.ProfilePicture.PictureURL });
				if (!success)
					throw new InvalidCastException("Could not delete old profile picture!");
			}

			string fileUrl = (await this._cloudService.UploadFilesToCloud(new List<IFormFile> { updateProfilePictureServiceModel.Picture }))[0] ??
				throw new ArgumentNullException("Unable to upload profile picture to cloud");

			bool successful = await this._userRepository.UpdateProfilePicture(updateProfilePictureServiceModel.UserId, fileUrl);

			if (!successful)
				throw new InvalidOperationException("Unable to change profile picture!");

			return new ProfilePictureServiceModel() { ProfilePictureURL = fileUrl };
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteUser(Guid id)
		{
			if (!await this._userRepository.DoesUserExistAsync(id))
				throw new ArgumentException("User does not exist!");

			User user = await this._userRepository.GetByIdAsync(id);
			bool result = await this._userRepository.DeleteAsync(user);

			return result;
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

		private string WriteJWTSecurityToken(Guid userId, string username, HashSet<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
			HashSet<Claim> claims = new()
			{
				new Claim("ID", $"{userId}"),
				new Claim("Username", username),
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

		#region Misc
		public async Task<TokenModel> SuperSecretPromotionToAdmin(Guid userId)
		{
			User user = await this._userRepository.GetByIdAsync(userId) ??
				throw new ArgumentException("User does not exist! Can't promote shit in this country...");

			if (!await this._roleRepository.DoesNameExist(Role.AdminRole))
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

			User newUser = await this._userRepository.GetByIdAsync(userId);

			return new TokenModel(WriteJWTSecurityToken(newUser.Id, newUser.UserName, newUser.Roles));
		}

		private async Task<User> PopulateModel(UpdateUserServiceModel updateUserServiceModel)
		{
			User user = this._userMapper.Map<User>(updateUserServiceModel);

			/* Fetch Roles and replace model's*/
			//Do NOT allow a user to change his roles, unless he is an Admin
			bool isAdmin = (await this._userRepository.GetByIdAsync(updateUserServiceModel.Id))
				.Roles.Any(r => r.Name == Role.AdminRole);

			if (isAdmin)
			{
				HashSet<Role> roles = new();
				foreach (var role in updateUserServiceModel.Roles)
				{
					Role returnedRole = await this._roleRepository.GetByNameAsync(role.Name) ??
						throw new ArgumentException($"Role {role.Name} does not exist!");

					roles.Add(returnedRole);
				}
				user.Roles = roles;
			}
			//Preserve original user roles
			else
				user.Roles = (await this._userRepository.GetByIdAsync(updateUserServiceModel.Id)).Roles;

			/* Fetch Languages and replace model's*/
			HashSet<Language> languages = new();
			int languagesCount = updateUserServiceModel.Languages.Count;
			for (int i = 0; i < languagesCount; i++)
			{
				Language language = await this._languageRepository.GetByNameAsync(updateUserServiceModel.Languages.ElementAt(i).Name) ??
					throw new ArgumentException("Invalid language name!");

				languages.Add(language);
			}
			user.Languages = languages;

			/* Fetch Technologies and replace model's*/
			HashSet<Technology> technologies = new();
			int technologiesCount = updateUserServiceModel.Technologies.Count;
			for (int i = 0; i < technologiesCount; i++)
			{
				Technology technology = await this._technologyRepository.GetByNameAsync(updateUserServiceModel.Technologies.ElementAt(i).Name) ??
					throw new ArgumentException("Invalid technology name!");

				technologies.Add(technology);
			}
			user.Technologies = technologies;

			return user;
		}

		private async Task<bool> CreateRelationToFriends(User user, List<UpdateFriendServiceModel> friends)
		{
			foreach (var friend in friends)
			{
				User amigo = await this._userRepository.GetByUsernameAsync(friend.UserName) ??
					throw new ArgumentException("No amigo, bro!");

				UserFriend relation = new()
				{
					UserId = user.Id,
					User = user,
					FriendId = amigo.Id,
					Friend = amigo
				};

				UserFriend theOtherRelation = new()
				{
					UserId = amigo.Id,
					User = amigo,
					FriendId = user.Id,
					Friend = user
				};

				user.MyFriends.Add(relation);
				user.FriendsOf.Add(theOtherRelation);
			}
			return true;
		}
		#endregion
	}
}
