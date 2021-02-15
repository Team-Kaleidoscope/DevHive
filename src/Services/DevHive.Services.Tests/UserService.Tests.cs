using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Models.Identity;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.User;
using DevHive.Services.Options;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
    [TestFixture]
	public class UserServiceTests
	{
		private Mock<ICloudService> CloudServiceMock { get; set; }
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IRoleRepository> RoleRepositoryMock { get; set; }
		private Mock<ILanguageRepository> LanguageRepositoryMock { get; set; }
		private Mock<ITechnologyRepository> TechnologyRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private JwtOptions JwtOptions { get; set; }
		private UserService UserService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.RoleRepositoryMock = new Mock<IRoleRepository>();
			this.CloudServiceMock = new Mock<ICloudService>();
			this.LanguageRepositoryMock = new Mock<ILanguageRepository>();
			this.TechnologyRepositoryMock = new Mock<ITechnologyRepository>();
			this.JwtOptions = new JwtOptions("gXfQlU6qpDleFWyimscjYcT3tgFsQg3yoFjcvSLxG56n1Vu2yptdIUq254wlJWjm");
			this.MapperMock = new Mock<IMapper>();
			// TODO: give actual UserManager and RoleManager to UserService
			this.UserService = new UserService(this.UserRepositoryMock.Object, this.LanguageRepositoryMock.Object, this.RoleRepositoryMock.Object, this.TechnologyRepositoryMock.Object, null, null, this.MapperMock.Object, this.JwtOptions, this.CloudServiceMock.Object);
		}
		#endregion

		#region LoginUser
		[Test]
		public async Task LoginUser_ReturnsTokenModel_WhenLoggingUserIn()
		{
			string somePassword = "GoshoTrapovImaGolemChep";
			const string name = "GoshoTrapov";
			string hashedPassword = PasswordModifications.GeneratePasswordHash(somePassword);
			LoginServiceModel loginServiceModel = new LoginServiceModel
			{
				Password = somePassword
			};
			User user = new User
			{
				Id = Guid.NewGuid(),
				PasswordHash = hashedPassword,
				UserName = name
			};

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
			this.UserRepositoryMock.Setup(p => p.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

			string JWTSecurityToken = this.WriteJWTSecurityToken(user.Id, user.UserName, user.Roles);

			TokenModel tokenModel = await this.UserService.LoginUser(loginServiceModel);

			Assert.AreEqual(JWTSecurityToken, tokenModel.Token, "LoginUser does not return the correct token");
		}

		[Test]
		public void LoginUser_ThrowsException_WhenUserNameDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "Invalid username!";
			LoginServiceModel loginServiceModel = new LoginServiceModel
			{
			};

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.LoginUser(loginServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorect Exception message");
		}

		[Test]
		public void LoginUser_ThroiwsException_WhenPasswordIsIncorect()
		{
			const string EXCEPTION_MESSAGE = "Incorrect password!";
			string somePassword = "GoshoTrapovImaGolemChep";
			LoginServiceModel loginServiceModel = new LoginServiceModel
			{
				Password = somePassword
			};
			User user = new User
			{
				Id = Guid.NewGuid(),
				PasswordHash = "InvalidPasswordHas"
			};

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
			this.UserRepositoryMock.Setup(p => p.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.LoginUser(loginServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorect Exception message");
		}
		#endregion

		#region RegisterUser
		// [Test]
		// public async Task RegisterUser_ReturnsTokenModel_WhenUserIsSuccessfull()
		// {
		// 	string somePassword = "GoshoTrapovImaGolemChep";
		// 	const string name = "GoshoTrapov";
		// 	RegisterServiceModel registerServiceModel = new RegisterServiceModel
		// 	{
		// 		Password = somePassword
		// 	};
		// 	User user = new User
		// 	{
		// 		Id = Guid.NewGuid(),
		// 		UserName = name
		// 	};
		// 	Role role = new Role { Name = Role.DefaultRole };
		// 	HashSet<Role> roles = new HashSet<Role> { role };
        //
		// 	this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
		// 	this.UserRepositoryMock.Setup(p => p.DoesEmailExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
		// 	this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(true));
		// 	this.RoleRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(role));
		// 	this.MapperMock.Setup(p => p.Map<User>(It.IsAny<RegisterServiceModel>())).Returns(user);
		// 	this.UserRepositoryMock.Setup(p => p.AddAsync(It.IsAny<User>())).Verifiable();
        //
		// 	string JWTSecurityToken = this.WriteJWTSecurityToken(user.Id, user.UserName, roles);
        //
		// 	TokenModel tokenModel = await this.UserService.RegisterUser(registerServiceModel);
        //
		// 	Mock.Verify();
		// 	Assert.AreEqual(JWTSecurityToken, tokenModel.Token, "RegisterUser does not return the correct token");
		// }

		[Test]
		public void RegisterUser_ThrowsException_WhenUsernameAlreadyExists()
		{
			const string EXCEPTION_MESSAGE = "Username already exists!";
			RegisterServiceModel registerServiceModel = new RegisterServiceModel
			{
			};

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.RegisterUser(registerServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorect Exception message");
		}

		[Test]
		public void RegisterUser_ThrowsException_WhenEmailAlreadyExists()
		{
			const string EXCEPTION_MESSAGE = "Email already exists!";

			RegisterServiceModel registerServiceModel = new RegisterServiceModel
			{
			};

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.UserRepositoryMock.Setup(p => p.DoesEmailExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.RegisterUser(registerServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorect Exception message");
		}
		#endregion

		#region GetUserById
		[Test]
		public async Task GetUserById_ReturnsTheUser_WhenItExists()
		{
			Guid id = new Guid();
			string name = "Gosho Trapov";
			User user = new()
			{
			};
			UserServiceModel userServiceModel = new UserServiceModel
			{
				UserName = name
			};

			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.MapperMock.Setup(p => p.Map<UserServiceModel>(It.IsAny<User>())).Returns(userServiceModel);

			UserServiceModel result = await this.UserService.GetUserById(id);

			Assert.AreEqual(name, result.UserName);
		}

		[Test]
		public void GetTechnologyById_ThrowsException_WhenTechnologyDoesNotExist()
		{
			const string EXCEPTION_MESSEGE = "User does not exist!";
			Guid id = new Guid();
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<User>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.GetUserById(id));

			Assert.AreEqual(EXCEPTION_MESSEGE, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetUserByUsername
		[Test]
		public async Task GetUserByUsername_ReturnsTheCorrectUser_IfItExists()
		{
			string username = "Gosho Trapov";
			User user = new User();
			UserServiceModel userServiceModel = new UserServiceModel
			{
				UserName = username
			};

			this.UserRepositoryMock.Setup(p => p.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
			this.MapperMock.Setup(p => p.Map<UserServiceModel>(It.IsAny<User>())).Returns(userServiceModel);

			UserServiceModel result = await this.UserService.GetUserByUsername(username);

			Assert.AreEqual(username, result.UserName, "GetUserByUsername does not return the correct user");
		}

		[Test]
		public async Task GetUserByUsername_ThrowsException_IfUserDoesNotExist()
		{
			string username = "Gosho Trapov";
			const string EXCEPTION_MESSEGE = "User does not exist!";

			this.UserRepositoryMock.Setup(p => p.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.GetUserByUsername(username));

			Assert.AreEqual(EXCEPTION_MESSEGE, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region UpdateUser
		// [Test]
		// [TestCase(true)]
		// [TestCase(false)]
		// public async Task UpdateUser_ReturnsIfUpdateIsSuccessfull_WhenUserExistsy(bool shouldPass)
		// {
		// 	string name = "Gosho Trapov";
		// 	Guid id = Guid.NewGuid();
		// 	User user = new User
		// 	{
		// 		UserName = name,
		// 		Id = id,
		// 	};
		// 	UpdateUserServiceModel updateUserServiceModel = new UpdateUserServiceModel
		// 	{
		// 		UserName = name,
		// 	};
		// 	UserServiceModel userServiceModel = new UserServiceModel
		// 	{
		// 		UserName = name,
		// 	};
		// 	Role role = new Role { };
        //
		// 	this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
		// 	this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
		// 	this.UserRepositoryMock.Setup(p => p.DoesUserHaveThisUsername(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
		// 	this.UserRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<User>())).Returns(Task.FromResult(shouldPass));
		// 	this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
		// 	this.MapperMock.Setup(p => p.Map<User>(It.IsAny<UpdateUserServiceModel>())).Returns(user);
		// 	this.MapperMock.Setup(p => p.Map<UserServiceModel>(It.IsAny<User>())).Returns(userServiceModel);
        //
		// 	if (shouldPass)
		// 	{
		// 		UserServiceModel result = await this.UserService.UpdateUser(updateUserServiceModel);
        //
		// 		Assert.AreEqual(updateUserServiceModel.UserName, result.UserName);
		// 	}
		// 	else
		// 	{
		// 		const string EXCEPTION_MESSAGE = "Unable to edit user!";
        //
		// 		Exception ex = Assert.ThrowsAsync<InvalidOperationException>(() => this.UserService.UpdateUser(updateUserServiceModel));
        //
		// 		Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorecct exception message");
		// 	}
		// }

		[Test]
		public void UpdateUser_ThrowsException_WhenUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "User does not exist!";
			UpdateUserServiceModel updateUserServiceModel = new UpdateUserServiceModel
			{
			};

			this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.UpdateUser(updateUserServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorecct exception message");
		}

		[Test]
		public void UpdateUser_ThrowsException_WhenUserNameAlreadyExists()
		{
			const string EXCEPTION_MESSAGE = "Username already exists!";
			UpdateUserServiceModel updateUserServiceModel = new UpdateUserServiceModel
			{
			};

			this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.UpdateUser(updateUserServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteUser
		//TO DO: compleate once Viko has looked into the return type of UserService.DeleteUser
		// [Test]
		// [TestCase(true)]
		// [TestCase(false)]
		// public async Task DeleteUser_ShouldReturnIfDeletionIsSuccessfull_WhenUserExists(bool shouldPass)
		// {
		// 	Guid id = new Guid();
		// 	User user = new User();
        //
		// 	this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
		// 	this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
		// 	this.UserRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<User>())).Returns(Task.FromResult(shouldPass));
        //
		// 	bool result = await this.UserService.DeleteUser(id);
        //
		// 	Assert.AreEqual(shouldPass, result);
		// }
        //
		[Test]
		public void DeleteUser_ThrowsException_WhenUserDoesNotExist()
		{
			string exceptionMessage = "User does not exist!";
			Guid id = new Guid();

			this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.UserService.DeleteUser(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region HelperMethods
		private string WriteJWTSecurityToken(Guid userId, string username, HashSet<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(this.JwtOptions.Secret);
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
	}
}
