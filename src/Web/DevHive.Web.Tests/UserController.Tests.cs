using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Common.Models.Identity;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.User;
using DevHive.Web.Controllers;
using DevHive.Web.Models.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class UserControllerTests
	{
		const string USERNAME = "Gosho Trapov";
		private Mock<IUserService> _userServiceMock;
		private Mock<IMapper> _mapperMock;
		private Mock<IJwtService> _jwtServiceMock;
		private UserController _userController;

		[SetUp]
		public void SetUp()
		{
			this._userServiceMock = new Mock<IUserService>();
			this._mapperMock = new Mock<IMapper>();
			this._jwtServiceMock = new Mock<IJwtService>();
			this._userController = new UserController(this._userServiceMock.Object, this._mapperMock.Object, this._jwtServiceMock.Object);
		}

		#region Create
		[Test]
		public void LoginUser_ReturnsOkObjectResult_WhenUserIsSuccessfullyLoggedIn()
		{
			LoginWebModel loginWebModel = new()
			{
				UserName = USERNAME
			};
			LoginServiceModel loginServiceModel = new()
			{
				UserName = USERNAME
			};
			string token = "goshotrapov";
			TokenModel tokenModel = new(token);
			TokenWebModel tokenWebModel = new(token);

			this._mapperMock.Setup(p => p.Map<LoginServiceModel>(It.IsAny<LoginWebModel>())).Returns(loginServiceModel);
			this._mapperMock.Setup(p => p.Map<TokenWebModel>(It.IsAny<TokenModel>())).Returns(tokenWebModel);
			this._userServiceMock.Setup(p => p.LoginUser(It.IsAny<LoginServiceModel>())).Returns(Task.FromResult(tokenModel));

			IActionResult result = this._userController.Login(loginWebModel).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			var resultToken = ((result as OkObjectResult).Value as TokenWebModel).Token;

			Assert.AreEqual(token, resultToken);
		}

		[Test]
		public void RegisterUser_ReturnsOkObjectResult_WhenUserIsSuccessfullyRegistered()
		{
			RegisterWebModel registerWebModel = new()
			{
				UserName = USERNAME
			};
			RegisterServiceModel registerServiceModel = new()
			{
				UserName = USERNAME
			};
			string token = "goshotrapov";
			TokenModel tokenModel = new(token);
			TokenWebModel tokenWebModel = new(token);

			this._mapperMock.Setup(p => p.Map<RegisterServiceModel>(It.IsAny<RegisterWebModel>())).Returns(registerServiceModel);
			this._mapperMock.Setup(p => p.Map<TokenWebModel>(It.IsAny<TokenModel>())).Returns(tokenWebModel);
			this._userServiceMock.Setup(p => p.RegisterUser(It.IsAny<RegisterServiceModel>())).Returns(Task.FromResult(tokenModel));

			IActionResult result = this._userController.Register(registerWebModel).Result;

			Assert.IsInstanceOf<CreatedResult>(result);

			CreatedResult createdResult = result as CreatedResult;
			TokenWebModel resultModel = (createdResult.Value as TokenWebModel);

			Assert.AreEqual(token, resultModel.Token);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsTheUser_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			UserServiceModel userServiceModel = new()
			{
				UserName = USERNAME
			};
			UserWebModel userWebModel = new()
			{
				UserName = USERNAME
			};

			this._userServiceMock.Setup(p => p.GetUserById(It.IsAny<Guid>())).Returns(Task.FromResult(userServiceModel));
			this._jwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this._mapperMock.Setup(p => p.Map<UserWebModel>(It.IsAny<UserServiceModel>())).Returns(userWebModel);

			IActionResult result = this._userController.GetById(id, null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			UserWebModel resultModel = okObjectResult.Value as UserWebModel;

			Assert.AreEqual(USERNAME, resultModel.UserName);
		}

		[Test]
		public void GetById_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			this._jwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(false);

			IActionResult result = this._userController.GetById(Guid.NewGuid(), null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}

		[Test]
		public void GetUser_ReturnsTheUser_WhenItExists()
		{
			UserWebModel userWebModel = new()
			{
				UserName = USERNAME
			};
			UserServiceModel userServiceModel = new()
			{
				UserName = USERNAME
			};

			this._userServiceMock.Setup(p => p.GetUserByUsername(It.IsAny<string>())).Returns(Task.FromResult(userServiceModel));
			this._mapperMock.Setup(p => p.Map<UserWebModel>(It.IsAny<UserServiceModel>())).Returns(userWebModel);

			IActionResult result = this._userController.GetUser(null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			UserWebModel resultModel = okObjectResult.Value as UserWebModel;

			Assert.AreEqual(USERNAME, resultModel.UserName);
		}
		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenUserIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdateUserWebModel updateUserWebModel = new()
			{
				UserName = USERNAME
			};
			UpdateUserServiceModel updateUserServiceModel = new()
			{
				UserName = USERNAME
			};
			UserServiceModel userServiceModel = new()
			{
				UserName = USERNAME
			};

			this._userServiceMock.Setup(p => p.UpdateUser(It.IsAny<UpdateUserServiceModel>())).Returns(Task.FromResult(userServiceModel));
			this._jwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this._mapperMock.Setup(p => p.Map<UpdateUserServiceModel>(It.IsAny<UpdateUserWebModel>())).Returns(updateUserServiceModel);

			IActionResult result = this._userController.Update(id, updateUserWebModel, null).Result;

			Assert.IsInstanceOf<AcceptedResult>(result);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenUserIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this._jwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this._userServiceMock.Setup(p => p.DeleteUser(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this._userController.Delete(id, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delete_ReturnsBadRequestObjectResult_WhenUserIsNotDeletedSuccessfully()
		{
			string message = "Could not delete User";
			Guid id = Guid.NewGuid();

			this._jwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this._userServiceMock.Setup(p => p.DeleteUser(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this._userController.Delete(id, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
