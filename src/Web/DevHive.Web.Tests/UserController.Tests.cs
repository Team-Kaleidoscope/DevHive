using System;
using System.Threading.Tasks;
using AutoMapper;
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
		private Mock<IUserService> UserServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private UserController UserController { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.UserServiceMock = new Mock<IUserService>();
			this.MapperMock = new Mock<IMapper>();
			this.UserController = new UserController(this.UserServiceMock.Object, this.MapperMock.Object);
		}

		#region Create
		[Test]
		public void LoginUser_ReturnsOkObjectResult_WhenUserIsSuccessfullyLoggedIn()
		{
			Guid id = Guid.NewGuid();
			LoginWebModel loginWebModel = new LoginWebModel
			{
				UserName = USERNAME
			};
			LoginServiceModel loginServiceModel = new LoginServiceModel
			{
				UserName = USERNAME
			};
			string token = "goshotrapov";
			TokenModel tokenModel = new TokenModel(token);
			TokenWebModel tokenWebModel = new TokenWebModel(token);

			this.MapperMock.Setup(p => p.Map<LoginServiceModel>(It.IsAny<LoginWebModel>())).Returns(loginServiceModel);
			this.MapperMock.Setup(p => p.Map<TokenWebModel>(It.IsAny<TokenModel>())).Returns(tokenWebModel);
			this.UserServiceMock.Setup(p => p.LoginUser(It.IsAny<LoginServiceModel>())).Returns(Task.FromResult(tokenModel));

			IActionResult result = this.UserController.Login(loginWebModel).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			var resultToken = ((result as OkObjectResult).Value as TokenWebModel).Token;

			Assert.AreEqual(token, resultToken);
		}

		[Test]
		public void RegisterUser_ReturnsOkObjectResult_WhenUserIsSuccessfullyRegistered()
		{
			Guid id = Guid.NewGuid();
			RegisterWebModel registerWebModel = new RegisterWebModel
			{
				UserName = USERNAME
			};
			RegisterServiceModel registerServiceModel = new RegisterServiceModel
			{
				UserName = USERNAME
			};
			string token = "goshotrapov";
			TokenModel tokenModel = new TokenModel(token);
			TokenWebModel tokenWebModel = new TokenWebModel(token);

			this.MapperMock.Setup(p => p.Map<RegisterServiceModel>(It.IsAny<RegisterWebModel>())).Returns(registerServiceModel);
			this.MapperMock.Setup(p => p.Map<TokenWebModel>(It.IsAny<TokenModel>())).Returns(tokenWebModel);
			this.UserServiceMock.Setup(p => p.RegisterUser(It.IsAny<RegisterServiceModel>())).Returns(Task.FromResult(tokenModel));

			IActionResult result = this.UserController.Register(registerWebModel).Result;

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

			UserServiceModel userServiceModel = new UserServiceModel
			{
				UserName = USERNAME
			};
			UserWebModel userWebModel = new UserWebModel
			{
				UserName = USERNAME
			};

			this.UserServiceMock.Setup(p => p.GetUserById(It.IsAny<Guid>())).Returns(Task.FromResult(userServiceModel));
			this.UserServiceMock.Setup(p => p.ValidJWT(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UserWebModel>(It.IsAny<UserServiceModel>())).Returns(userWebModel);

			IActionResult result = this.UserController.GetById(id, null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			UserWebModel resultModel = okObjectResult.Value as UserWebModel;

			Assert.AreEqual(USERNAME, resultModel.UserName);
		}

		[Test]
		public void GetById_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			Guid id = Guid.NewGuid();
			UserWebModel userWebModel = new UserWebModel
			{
				UserName = USERNAME
			};

			this.UserServiceMock.Setup(p => p.ValidJWT(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.UserController.GetById(Guid.NewGuid(), null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}

		[Test]
		public void GetUser_ReturnsTheUser_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			UserWebModel userWebModel = new UserWebModel
			{
				UserName = USERNAME
			};
			UserServiceModel userServiceModel = new UserServiceModel
			{
				UserName = USERNAME
			};

			this.UserServiceMock.Setup(p => p.GetUserByUsername(It.IsAny<string>())).Returns(Task.FromResult(userServiceModel));
			this.MapperMock.Setup(p => p.Map<UserWebModel>(It.IsAny<UserServiceModel>())).Returns(userWebModel);

			IActionResult result = this.UserController.GetUser(null).Result;

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
			UpdateUserWebModel updateUserWebModel = new UpdateUserWebModel
			{
				UserName = USERNAME
			};
			UpdateUserServiceModel updateUserServiceModel = new UpdateUserServiceModel
			{
				UserName = USERNAME
			};
			UserServiceModel userServiceModel = new UserServiceModel
			{
				UserName = USERNAME
			};

			this.UserServiceMock.Setup(p => p.UpdateUser(It.IsAny<UpdateUserServiceModel>())).Returns(Task.FromResult(userServiceModel));
			this.UserServiceMock.Setup(p => p.ValidJWT(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateUserServiceModel>(It.IsAny<UpdateUserWebModel>())).Returns(updateUserServiceModel);

			IActionResult result = this.UserController.Update(id, updateUserWebModel, null).Result;

			Assert.IsInstanceOf<AcceptedResult>(result);
		}

		[Test]
		public void UpdateProfilePicture_ShouldReturnOkObjectResult_WhenProfilePictureIsUpdatedSuccessfully()
		{
			string profilePictureURL = "goshotrapov";
			UpdateProfilePictureWebModel updateProfilePictureWebModel = new UpdateProfilePictureWebModel();
			UpdateProfilePictureServiceModel updateProfilePictureServiceModel = new UpdateProfilePictureServiceModel();
			ProfilePictureServiceModel profilePictureServiceModel = new ProfilePictureServiceModel
			{
				ProfilePictureURL = profilePictureURL
			};
			ProfilePictureWebModel profilePictureWebModel = new ProfilePictureWebModel
			{
				ProfilePictureURL = profilePictureURL
			};

			this.UserServiceMock.Setup(p => p.ValidJWT(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateProfilePictureServiceModel>(It.IsAny<UpdateProfilePictureWebModel>())).Returns(updateProfilePictureServiceModel);
			this.UserServiceMock.Setup(p => p.UpdateProfilePicture(It.IsAny<UpdateProfilePictureServiceModel>())).Returns(Task.FromResult(profilePictureServiceModel));
			this.MapperMock.Setup(p => p.Map<ProfilePictureWebModel>(It.IsAny<ProfilePictureServiceModel>())).Returns(profilePictureWebModel);


			IActionResult result = this.UserController.UpdateProfilePicture(Guid.Empty, updateProfilePictureWebModel, null).Result;

			Assert.IsInstanceOf<AcceptedResult>(result);

			AcceptedResult acceptedResult = result as AcceptedResult;
			ProfilePictureWebModel resultModel = acceptedResult.Value as ProfilePictureWebModel;

			Assert.AreEqual(profilePictureURL, resultModel.ProfilePictureURL);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenUserIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this.UserServiceMock.Setup(p => p.ValidJWT(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.UserServiceMock.Setup(p => p.DeleteUser(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this.UserController.Delete(id, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delete_ReturnsBadRequestObjectResult_WhenUserIsNotDeletedSuccessfully()
		{
			string message = "Could not delete User";
			Guid id = Guid.NewGuid();

			this.UserServiceMock.Setup(p => p.ValidJWT(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.UserServiceMock.Setup(p => p.DeleteUser(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this.UserController.Delete(id, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
