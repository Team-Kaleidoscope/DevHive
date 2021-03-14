using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Common.Models.Identity;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.User;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
    [TestFixture]
	public class UserServiceTests
	{
		private Mock<ICloudService> _cloudServiceMock;
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<IRoleRepository> _roleRepositoryMock;
		private Mock<ILanguageRepository> _languageRepositoryMock;
		private Mock<ITechnologyRepository> _technologyRepositoryMock;
		private Mock<IMapper> _mapperMock;
		private Mock<IJwtService> _jwtServiceMock;
		private UserService _userService;

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this._userRepositoryMock = new Mock<IUserRepository>();
			this._roleRepositoryMock = new Mock<IRoleRepository>();
			this._cloudServiceMock = new Mock<ICloudService>();
			this._languageRepositoryMock = new Mock<ILanguageRepository>();
			this._technologyRepositoryMock = new Mock<ITechnologyRepository>();
			this._jwtServiceMock = new Mock<IJwtService>();
			this._mapperMock = new Mock<IMapper>();
			this._userService = new UserService(
				this._userRepositoryMock.Object,
				this._languageRepositoryMock.Object,
				this._roleRepositoryMock.Object,
				this._technologyRepositoryMock.Object,
				this._mapperMock.Object,
				this._cloudServiceMock.Object,
				this._jwtServiceMock.Object);
		}
		#endregion

		#region LoginUser
		[Test]
		public async Task LoginUser_ReturnsTokenModel_WhenLoggingUserIn()
		{
			string somePassword = "I'm_Nigga";

			LoginServiceModel loginServiceModel = new()
			{
				Password = somePassword
			};
			User user = new()
			{
				Id = Guid.NewGuid(),
				PasswordHash = somePassword,
				UserName = "g_trapov"
			};

			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(true));
			this._userRepositoryMock
				.Setup(p => p.VerifyPassword(It.IsAny<User>(), It.IsAny<string>()))
				.Returns(Task.FromResult(true));
			this._userRepositoryMock
				.Setup(p => p.GetByUsernameAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(user));

			string jwtSecurityToken = "akjdhfakndvlahdfkljahdlfkjhasldf";
			this._jwtServiceMock
				.Setup(p => p.GenerateJwtToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.Returns(jwtSecurityToken);
			TokenModel tokenModel = await this._userService.LoginUser(loginServiceModel);

			Assert.AreEqual(jwtSecurityToken, tokenModel.Token, "LoginUser does not return the correct token");
		}

		[Test]
		public void LoginUser_ThrowsException_WhenUserNameDoesNotExist()
		{
			LoginServiceModel loginServiceModel = new();

			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(
				() => this._userService.LoginUser(loginServiceModel));

			Assert.AreEqual("Invalid username!", ex.Message, "Incorrect Exception message");
		}

		[Test]
		public void LoginUser_ThrowsException_WhenPasswordIsIncorrect()
		{
			string somePassword = "I'm_Nigga";

			LoginServiceModel loginServiceModel = new()
			{
				Password = somePassword
			};
			User user = new()
			{
				Id = Guid.NewGuid(),
			};

			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(true));
			this._userRepositoryMock
				.Setup(p => p.GetByUsernameAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(user));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.LoginUser(loginServiceModel));

			Assert.AreEqual("Incorrect password!", ex.Message, "Incorrect Exception message");
		}
		#endregion

		#region RegisterUser
		[Test]
		public async Task RegisterUser_ReturnsTokenModel_WhenUserIsSuccessfull()
		{
			Guid userId = Guid.NewGuid();
			RegisterServiceModel registerServiceModel = new()
			{
				Password = "ImNigga"
			};
			User user = new()
			{
				Id = userId,
				UserName = "g_trapov"
			};
			Role role = new() { Name = Role.DefaultRole };

			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(false));
			this._userRepositoryMock
				.Setup(p => p.VerifyPassword(It.IsAny<User>(), It.IsAny<string>()))
				.Returns(Task.FromResult(true));
			this._userRepositoryMock
				.Setup(p => p.DoesEmailExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(false));
			this._userRepositoryMock
				.Setup(p => p.AddAsync(It.IsAny<User>()))
				.ReturnsAsync(true);
			this._userRepositoryMock
				.Setup(p => p.AddRoleToUser(It.IsAny<User>(), It.IsAny<string>()))
				.ReturnsAsync(true);
			this._userRepositoryMock
				.Setup(p => p.GetByUsernameAsync(It.IsAny<string>()))
				.ReturnsAsync(user);

			this._roleRepositoryMock
				.Setup(p => p.DoesNameExist(It.IsAny<string>()))
				.Returns(Task.FromResult(true));
			this._roleRepositoryMock
				.Setup(p => p.GetByNameAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(role));

			this._mapperMock
				.Setup(p => p.Map<User>(It.IsAny<RegisterServiceModel>()))
				.Returns(user);

			string jwtSecurityToken = "akjdhfakndvlahdfkljahdlfkjhasldf";
			this._jwtServiceMock
				.Setup(p => p.GenerateJwtToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>()))
				.Returns(jwtSecurityToken);
			TokenModel tokenModel = await this._userService.RegisterUser(registerServiceModel);

			Assert.AreEqual(jwtSecurityToken, tokenModel.Token, "RegisterUser does not return the correct token");
		}

		[Test]
		public void RegisterUser_ThrowsException_WhenUsernameAlreadyExists()
		{
			const string EXCEPTION_MESSAGE = "Username already exists!";
			RegisterServiceModel registerServiceModel = new();

			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(
				() => this._userService.RegisterUser(registerServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorrect Exception message");
		}

		[Test]
		public void RegisterUser_ThrowsException_WhenEmailAlreadyExists()
		{
			RegisterServiceModel registerServiceModel = new();

			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(false));
			this._userRepositoryMock
				.Setup(p => p.DoesEmailExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.RegisterUser(registerServiceModel));

			Assert.AreEqual("Email already exists!", ex.Message, "Incorrect Exception message");
		}
		#endregion

		#region GetUserById
		[Test]
		public async Task GetUserById_ReturnsTheUser_WhenItExists()
		{
			Guid id = new();
			string username = "g_trapov";
			User user = new();
			UserServiceModel userServiceModel = new()
			{
				UserName = username
			};

			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(user));
			this._mapperMock
				.Setup(p => p.Map<UserServiceModel>(It.IsAny<User>()))
				.Returns(userServiceModel);

			UserServiceModel result = await this._userService.GetUserById(id);

			Assert.AreEqual(username, result.UserName);
		}

		[Test]
		public void GetTechnologyById_ThrowsException_WhenTechnologyDoesNotExist()
		{
			Guid id = new();
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult<User>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.GetUserById(id));

			Assert.AreEqual("User does not exist!", ex.Message, "Incorrect exception message");
		}
		#endregion

		#region GetUserByUsername
		[Test]
		public async Task GetUserByUsername_ReturnsTheCorrectUser_IfItExists()
		{
			string username = "g_trapov";
			User user = new();
			UserServiceModel userServiceModel = new()
			{
				UserName = username
			};

			this._userRepositoryMock
				.Setup(p => p.GetByUsernameAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(user));
			this._mapperMock
				.Setup(p => p.Map<UserServiceModel>(It.IsAny<User>()))
				.Returns(userServiceModel);

			UserServiceModel result = await this._userService.GetUserByUsername(username);

			Assert.AreEqual(username, result.UserName, "GetUserByUsername does not return the correct user");
		}

		[Test]
		public void GetUserByUsername_ThrowsException_IfUserDoesNotExist()
		{
			string username = "g_trapov";

			this._userRepositoryMock
				.Setup(p => p.GetByUsernameAsync(It.IsAny<string>()))
				.Returns(Task.FromResult<User>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.GetUserByUsername(username));

			Assert.AreEqual("User does not exist!", ex.Message, "Incorrect exception message");
		}
		#endregion

		#region UpdateUser
		// [Test]
		// [TestCase(true)]
		// [TestCase(false)]
		// public async Task UpdateUser_ReturnsIfUpdateIsSuccessfull_WhenUserExistsy(bool shouldPass)
		// {
		// 	string username = "g_trapov";
		// 	Guid id = Guid.NewGuid();
		// 	User user = new User
		// 	{
		// 		UserName = username,
		// 		Id = id,
		// 	};
		// 	UpdateUserServiceModel updateUserServiceModel = new UpdateUserServiceModel
		// 	{
		// 		UserName = username,
		// 	};
		// 	UserServiceModel userServiceModel = new UserServiceModel
		// 	{
		// 		UserName = username,
		// 	};
		// 	Role role = new Role { };
		//
		// 	this._userRepositoryMock.Setup(p =>
		// p.DoesUserExistAsync(It.IsAny<Guid>()))
		// 	.Returns(Task.FromResult(true));
		// 	this._userRepositoryMock.Setup(p =>
		// p.DoesUsernameExistAsync(It.IsAny<string>()))
		// 	.Returns(Task.FromResult(false));
		// 	this._userRepositoryMock.Setup(p =>
		// p.DoesUserHaveThisUsernameAsync(It.IsAny<Guid>(), It.IsAny<string>()))
		// 	.Returns(true);
		// 	this._userRepositoryMock.Setup(p =>
		// p.EditAsync(It.IsAny<Guid>(), It.IsAny<User>()))
		// 	.Returns(Task.FromResult(shouldPass));
		// 	this._userRepositoryMock.Setup(p =>
		// p.GetByIdAsync(It.IsAny<Guid>()))
		// 	.Returns(Task.FromResult(user));
		// 	this._mapperMock.Setup(p =>
		// p.Map<User>(It.IsAny<UpdateUserServiceModel>()))
		// 	.Returns(user);
		// 	this._mapperMock.Setup(p =>
		// p.Map<UserServiceModel>(It.IsAny<User>()))
		// 	.Returns(userServiceModel);
		//
		// 	if (shouldPass)
		// 	{
		// 		UserServiceModel result = await this._userService.UpdateUser(updateUserServiceModel);
		//
		// 		Assert.AreEqual(updateUserServiceModel.UserName, result.UserName);
		// 	}
		// 	else
		// 	{
		// 		const string EXCEPTION_MESSAGE = "Unable to edit user!";
		//
		// 		Exception ex = Assert.ThrowsAsync<InvalidOperationException>(() => this._userService.UpdateUser(updateUserServiceModel));
		//
		// 		Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorrect exception message");
		// 	}
		// }

		[Test]
		public void UpdateUser_ThrowsException_WhenUserDoesNotExist()
		{
			UpdateUserServiceModel updateUserServiceModel = new();

			this._userRepositoryMock
				.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.UpdateUser(updateUserServiceModel));

			Assert.AreEqual("User does not exist!", ex.Message, "Incorrect exception message");
		}

		[Test]
		public void UpdateUser_ThrowsException_WhenUserNameAlreadyExists()
		{
			UpdateUserServiceModel updateUserServiceModel = new();

			this._userRepositoryMock
				.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(true));
			this._userRepositoryMock
				.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.UpdateUser(updateUserServiceModel));

			Assert.AreEqual("Username already exists!", ex.Message, "Incorrect exception message");
		}
		#endregion

		#region DeleteUser
		//TO DO: compleate once Viko has looked into the return type of UserService.DeleteUser
		// [Test]
		// [TestCase(true)]
		// [TestCase(false)]
		// public async Task DeleteUser_ShouldReturnIfDeletionIsSuccessfull_WhenUserExists(bool shouldPass)
		// {
		// 	Guid id = Guid.NewGuid();
		// 	User user = new User();
		//
		// 	this._userRepositoryMock.Setup(p =>
		// p.DoesUserExistAsync(It.IsAny<Guid>()))
		// 			.Returns(Task.FromResult(true));
		// 	this._userRepositoryMock.Setup(p =>
		// p.GetByIdAsync(It.IsAny<Guid>()))
		// 	.Returns(Task.FromResult(user));
		// 	this._userRepositoryMock.Setup(p =>
		// p.DeleteAsync(It.IsAny<User>()))
		// 			.Returns(Task.FromResult(shouldPass));
		//
		// 	bool result = await this._userService.DeleteUser(id);
		//
		// 	Assert.AreEqual(shouldPass, result);
		// }
		//
		[Test]
		public void DeleteUser_ThrowsException_WhenUserDoesNotExist()
		{
			string exceptionMessage = "User does not exist!";
			Guid id = new();

			this._userRepositoryMock
				.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._userService.DeleteUser(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorrect exception message");
		}
		#endregion
	}
}
