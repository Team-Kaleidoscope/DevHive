using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Constants;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Common.Models.Identity;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.User;

namespace DevHive.Services.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly ILanguageRepository _languageRepository;
		private readonly ITechnologyRepository _technologyRepository;
		private readonly IMapper _userMapper;
		private readonly IJwtService _jwtService;

		private const string NoYourselfAsFriend = "You cant add yourself as a friend(sry, bro)!";
		private const string Rant = "Can't promote shit in this country...";


		public UserService(IUserRepository userRepository,
			ILanguageRepository languageRepository,
			IRoleRepository roleRepository,
			ITechnologyRepository technologyRepository,
			IMapper mapper,
			IJwtService jwtService)
		{
			this._userRepository = userRepository;
			this._roleRepository = roleRepository;
			this._userMapper = mapper;
			this._languageRepository = languageRepository;
			this._technologyRepository = technologyRepository;
			this._jwtService = jwtService;
		}

		#region Authentication
		public async Task<TokenModel> LoginUser(LoginServiceModel loginModel)
		{
			if (!await this._userRepository.DoesUsernameExistAsync(loginModel.UserName))
				throw new InvalidDataException(string.Format(ErrorMessages.InvalidData, ClassesConstants.Username));

			User user = await this._userRepository.GetByUsernameAsync(loginModel.UserName);

			if (!await this._userRepository.VerifyPassword(user, loginModel.Password))
				throw new InvalidDataException(string.Format(ErrorMessages.IncorrectData, ClassesConstants.Password.ToLower()));

			List<string> roleNames = user.Roles.Select(x => x.Name).ToList();
			return new TokenModel(this._jwtService.GenerateJwtToken(user.Id, user.UserName, roleNames));
		}

		public async Task<TokenModel> RegisterUser(RegisterServiceModel registerModel)
		{
			if (await this._userRepository.DoesUsernameExistAsync(registerModel.UserName))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Username));

			if (await this._userRepository.DoesEmailExistAsync(registerModel.Email))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Email));


			User user = this._userMapper.Map<User>(registerModel);

			bool userResult = await this._userRepository.AddAsync(user);
			bool roleResult = await this._userRepository.AddRoleToUser(user, Role.DefaultRole);

			if (!userResult)
				throw new InvalidOperationException(string.Format(ErrorMessages.CannotCreate, ClassesConstants.User.ToLower()));
			if (!roleResult)
				throw new InvalidOperationException(string.Format(ErrorMessages.CannotAdd, ClassesConstants.Role.ToLower()));

			User createdUser = await this._userRepository.GetByUsernameAsync(registerModel.UserName);

			List<string> roleNames = createdUser.Roles.Select(x => x.Name).ToList();
			return new TokenModel(this._jwtService.GenerateJwtToken(createdUser.Id, createdUser.UserName, roleNames));
		}
		#endregion

		#region Read
		public async Task<UserServiceModel> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User));

			return this._userMapper.Map<UserServiceModel>(user);
		}

		public async Task<UserServiceModel> GetUserByUsername(string username)
		{
			User user = await this._userRepository.GetByUsernameAsync(username) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User));

			return this._userMapper.Map<UserServiceModel>(user);
		}
		#endregion

		#region Update
		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateUserServiceModel)
		{
			await ValidateUserOnUpdate(updateUserServiceModel);

			User user = await this._userRepository.GetByIdAsync(updateUserServiceModel.Id);
			await PopulateUserModel(user, updateUserServiceModel);

			if (updateUserServiceModel.Friends.Count > 0)
				await CreateRelationToFriends(user, updateUserServiceModel.Friends.ToList());
			else
				user.Friends.Clear();

			bool result = await this._userRepository.EditAsync(user.Id, user);

			if (!result)
				throw new InvalidOperationException(string.Format(ErrorMessages.CannotEdit, ClassesConstants.User.ToLower()));

			User newUser = await this._userRepository.GetByIdAsync(user.Id);
			return this._userMapper.Map<UserServiceModel>(newUser);
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteUser(Guid id)
		{
			if (!await this._userRepository.DoesUserExistAsync(id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User));

			User user = await this._userRepository.GetByIdAsync(id);
			return await this._userRepository.DeleteAsync(user);
		}
		#endregion

		#region Validations
		/// <summary>
		/// Checks whether the user in the model exists and whether the username in the model is already taken.
		/// If the check fails (is false), it throws an exception, otherwise nothing happens
		/// </summary>
		private async Task ValidateUserOnUpdate(UpdateUserServiceModel updateUserServiceModel)
		{
			if (!await this._userRepository.DoesUserExistAsync(updateUserServiceModel.Id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User));

			if (updateUserServiceModel.Friends.Any(x => x.UserName == updateUserServiceModel.UserName))
				throw new InvalidOperationException(NoYourselfAsFriend);

			if (!await this._userRepository.DoesUserHaveThisUsernameAsync(updateUserServiceModel.Id, updateUserServiceModel.UserName)
					&& await this._userRepository.DoesUsernameExistAsync(updateUserServiceModel.UserName))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Username.ToLower()));

			List<string> usernames = new();
			foreach (var friend in updateUserServiceModel.Friends)
				usernames.Add(friend.UserName);

			if (!await this._userRepository.ValidateFriendsCollectionAsync(usernames))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.OneOrMoreFriends));
		}
		#endregion

		#region Misc
		public async Task<TokenModel> SuperSecretPromotionToAdmin(Guid userId)
		{
			User user = await this._userRepository.GetByIdAsync(userId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.User) + " " + Rant);

			if (!await this._roleRepository.DoesNameExist(Role.AdminRole))
			{
				Role adminRole = new() { Name = Role.AdminRole };
				adminRole.Users.Add(user);

				await this._roleRepository.AddAsync(adminRole);
			}

			Role admin = await this._roleRepository.GetByNameAsync(Role.AdminRole);

			user.Roles.Add(admin);
			await this._userRepository.EditAsync(user.Id, user);

			User createdUser = await this._userRepository.GetByIdAsync(userId);
			List<string> roleNames = createdUser
						.Roles
						.Select(x => x.Name)
						.ToList();

			return new TokenModel(this._jwtService.GenerateJwtToken(createdUser.Id, createdUser.UserName, roleNames));
		}

		private async Task PopulateUserModel(User user, UpdateUserServiceModel updateUserServiceModel)
		{
			user.UserName = updateUserServiceModel.UserName;
			user.FirstName = updateUserServiceModel.FirstName;
			user.LastName = updateUserServiceModel.LastName;
			user.Email = updateUserServiceModel.Email;

			//Do NOT allow a user to change his roles, unless he is an Admin
			bool isAdmin = await this._userRepository.IsInRoleAsync(user, Role.AdminRole);

			if (isAdmin)
			{
				HashSet<Role> roles = new();
				foreach (var role in updateUserServiceModel.Roles)
				{
					Role returnedRole = await this._roleRepository.GetByNameAsync(role.Name) ??
						throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Role));

					roles.Add(returnedRole);
				}
				user.Roles = roles;
			}

			HashSet<Language> languages = new();
			int languagesCount = updateUserServiceModel.Languages.Count;
			for (int i = 0; i < languagesCount; i++)
			{
				Language language = await this._languageRepository.GetByNameAsync(updateUserServiceModel.Languages.ElementAt(i).Name) ??
					throw new InvalidDataException(string.Format(ErrorMessages.InvalidData, nameof(Language)));

				languages.Add(language);
			}
			user.Languages = languages;

			/* Fetch Technologies and replace model's*/
			HashSet<Technology> technologies = new();
			int technologiesCount = updateUserServiceModel.Technologies.Count;
			for (int i = 0; i < technologiesCount; i++)
			{
				Technology technology = await this._technologyRepository.GetByNameAsync(updateUserServiceModel.Technologies.ElementAt(i).Name) ??
					throw new InvalidDataException(string.Format(ErrorMessages.InvalidData, nameof(Technology)));


				technologies.Add(technology);
			}
			user.Technologies = technologies;
		}

		private async Task CreateRelationToFriends(User user, List<UpdateFriendServiceModel> friends)
		{
			foreach (var friend in friends)
			{
				User amigo = await this._userRepository.GetByUsernameAsync(friend.UserName);

				user.Friends.Add(amigo);
				amigo.Friends.Add(user);

				await this._userRepository.EditAsync(amigo.Id, amigo);
			}
		}
		#endregion
	}
}
