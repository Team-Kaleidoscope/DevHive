using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Models.Identity;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models.Identity.User;
using DevHive.Services.Options;
using DevHive.Services.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class UserServiceTests
	{
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IRoleRepository> RoleRepositoryMock { get; set; }
		private Mock<ILanguageRepository> LanguageRepositoryMock { get; set; }
		private Mock<ITechnologyRepository> TechnologyRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private JWTOptions JWTOptions { get; set; }
		private UserService UserService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.RoleRepositoryMock = new Mock<IRoleRepository>();
			this.LanguageRepositoryMock = new Mock<ILanguageRepository>();
			this.TechnologyRepositoryMock = new Mock<ITechnologyRepository>();
			this.JWTOptions = new JWTOptions("gXfQlU6qpDleFWyimscjYcT3tgFsQg3yoFjcvSLxG56n1Vu2yptdIUq254wlJWjm");
			this.MapperMock = new Mock<IMapper>();
			this.UserService = new UserService(this.UserRepositoryMock.Object, this.LanguageRepositoryMock.Object, this.RoleRepositoryMock.Object, this.TechnologyRepositoryMock.Object, this.MapperMock.Object, JWTOptions);
		}
		#endregion

		#region LoginUser
		[Test]
		public async Task LoginUser_ReturnsTokenModel_WhenLoggingUserIn()
		{
			string somePassword = "GoshoTrapovImaGolemChep";
			string hashedPassword = PasswordModifications.GeneratePasswordHash(somePassword);
			LoginServiceModel loginServiceModel = new LoginServiceModel
			{
				Password = somePassword
			};
			User user = new User
			{
				Id = Guid.NewGuid(),
				PasswordHash = hashedPassword
			};

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
			this.UserRepositoryMock.Setup(p => p.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(user));

			string JWTSecurityToken = this.WriteJWTSecurityToken(user.Id, user.Roles);

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
		[Test]
		public async Task RegisterUser_ReturnsTokenModel_WhenUserIsSuccessfull()
		{
			string somePassword = "GoshoTrapovImaGolemChep";
			RegisterServiceModel registerServiceModel = new RegisterServiceModel
			{
				Password = somePassword
			};
			User user = new User
			{
				Id = Guid.NewGuid()
			};
			Role role = new Role { Name = Role.DefaultRole };
			HashSet<Role> roles = new HashSet<Role> { role };

			this.UserRepositoryMock.Setup(p => p.DoesUsernameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.UserRepositoryMock.Setup(p => p.DoesEmailExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(true));
			this.RoleRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(role));
			this.MapperMock.Setup(p => p.Map<User>(It.IsAny<RegisterServiceModel>())).Returns(user);
			this.UserRepositoryMock.Setup(p => p.AddAsync(It.IsAny<User>())).Verifiable();

			string JWTSecurityToken = this.WriteJWTSecurityToken(user.Id, roles);

			TokenModel tokenModel = await this.UserService.RegisterUser(registerServiceModel);

			Mock.Verify();
			Assert.AreEqual(JWTSecurityToken, tokenModel.Token, "RegisterUser does not return the correct token");
		}
		#endregion

		#region HelperMethods
		private string WriteJWTSecurityToken(Guid userId, HashSet<Role> roles)
		{
			byte[] signingKey = Encoding.ASCII.GetBytes(this.JWTOptions.Secret);

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
	}
}
