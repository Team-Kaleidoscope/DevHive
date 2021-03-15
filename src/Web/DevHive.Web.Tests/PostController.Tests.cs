using System;
using System.Linq;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Post;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class PostControllerTests
	{
		const string MESSAGE = "Gosho Trapov";
		private Mock<IPostService> _postServiceMock;
		private Mock<IMapper> _mapperMock;
		private Mock<IJwtService> _jwtServiceMock;
		private PostController _postController;

		[SetUp]
		public void SetUp()
		{
			this._postServiceMock = new Mock<IPostService>();
			this._mapperMock = new Mock<IMapper>();
			this._jwtServiceMock = new Mock<IJwtService>();
			this._postController = new PostController(this._postServiceMock.Object, this._mapperMock.Object, this._jwtServiceMock.Object);
		}

		#region Create
		[Test]
		public void CreatePost_ReturnsOkObjectResult_WhenPostIsSuccessfullyCreated()
		{
			CreatePostWebModel createPostWebModel = new()
			{
				Message = MESSAGE
			};
			CreatePostServiceModel createPostServiceModel = new()
			{
				Message = MESSAGE
			};
			Guid id = Guid.NewGuid();

			this._mapperMock
				.Setup(p => p.Map<CreatePostServiceModel>(It.IsAny<CreatePostWebModel>()))
				.Returns(createPostServiceModel);
			this._postServiceMock
				.Setup(p => p.CreatePost(It.IsAny<CreatePostServiceModel>()))
				.ReturnsAsync(id);
			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._postServiceMock
				.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			IActionResult result = this._postController.Create(Guid.Empty, createPostWebModel, null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			var splitted = (result as OkObjectResult).Value
				.ToString()
				.Split('{', '}', '=', ' ')
				.Where(x => !string.IsNullOrEmpty(x))
				.ToArray();

			Guid resultId = Guid.Parse(splitted[1]);

			Assert.AreEqual(id, resultId);
		}

		[Test]
		public void CreatePost_ReturnsBadRequestObjectResult_WhenPostIsNotCreatedSuccessfully()
		{
			CreatePostWebModel createTechnologyWebModel = new()
			{
				Message = MESSAGE
			};
			CreatePostServiceModel createTechnologyServiceModel = new()
			{
				Message = MESSAGE
			};
			Guid id = Guid.Empty;
			string errorMessage = $"Could not create post!";

			this._mapperMock
				.Setup(p => p.Map<CreatePostServiceModel>(It.IsAny<CreatePostWebModel>()))
				.Returns(createTechnologyServiceModel);
			this._postServiceMock
				.Setup(p => p.CreatePost(It.IsAny<CreatePostServiceModel>()))
				.ReturnsAsync(id);
			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._postServiceMock
				.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			IActionResult result = this._postController.Create(Guid.Empty, createTechnologyWebModel, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}

		[Test]
		public void CreatePost_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			CreatePostWebModel createPostWebModel = new()
			{
				Message = MESSAGE
			};

			this._postServiceMock
				.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(false);

			IActionResult result = this._postController.Create(Guid.NewGuid(), createPostWebModel, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsThePost_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			ReadPostServiceModel readPostServiceModel = new()
			{
				Message = MESSAGE
			};
			ReadPostWebModel readPostWebModel = new()
			{
				Message = MESSAGE
			};

			this._postServiceMock
				.Setup(p => p.GetPostById(It.IsAny<Guid>()))
				.ReturnsAsync(readPostServiceModel);
			this._mapperMock
				.Setup(p => p.Map<ReadPostWebModel>(It.IsAny<ReadPostServiceModel>()))
				.Returns(readPostWebModel);

			IActionResult result = this._postController.GetById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadPostWebModel resultModel = okObjectResult.Value as Models.Post.ReadPostWebModel;

			Assert.AreEqual(MESSAGE, resultModel.Message);
		}
		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenPostIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdatePostWebModel updatePostWebModel = new()
			{
				NewMessage = MESSAGE
			};
			UpdatePostServiceModel updatePostServiceModel = new()
			{
				NewMessage = MESSAGE
			};

			this._postServiceMock
				.Setup(p => p.UpdatePost(It.IsAny<UpdatePostServiceModel>()))
				.ReturnsAsync(id);
			this._mapperMock
				.Setup(p => p.Map<UpdatePostServiceModel>(It.IsAny<UpdatePostWebModel>()))
				.Returns(updatePostServiceModel);
			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._postServiceMock
				.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			IActionResult result = this._postController.Update(id, updatePostWebModel, null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenPostIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			string message = "Could not update post!";
			UpdatePostWebModel updatePostWebModel = new()
			{
				NewMessage = MESSAGE
			};
			UpdatePostServiceModel updatePostServiceModel = new()
			{
				NewMessage = MESSAGE
			};

			this._postServiceMock
				.Setup(p => p.UpdatePost(It.IsAny<UpdatePostServiceModel>()))
				.ReturnsAsync(Guid.Empty);
			this._mapperMock
				.Setup(p => p.Map<UpdatePostServiceModel>(It.IsAny<UpdatePostWebModel>()))
				.Returns(updatePostServiceModel);
			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._postServiceMock
				.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			IActionResult result = this._postController.Update(id, updatePostWebModel, null).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void Update_ShouldReturnUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			UpdatePostWebModel updatePostWebModel = new()
			{
				NewMessage = MESSAGE
			};

			this._postServiceMock
				.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(false);

			IActionResult result = this._postController.Update(Guid.Empty, updatePostWebModel, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenPostIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this._postServiceMock
				.Setup(p => p.DeletePost(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._postServiceMock
				.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			IActionResult result = this._postController.Delete(id, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delete_ReturnsBadRequestObjectResult_WhenPostIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Post";
			Guid id = Guid.NewGuid();

			this._postServiceMock
				.Setup(p => p.DeletePost(It.IsAny<Guid>()))
				.ReturnsAsync(false);
			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._postServiceMock
				.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			IActionResult result = this._postController.Delete(id, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void DeletePost_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			this._postServiceMock
				.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>()))
				.ReturnsAsync(false);

			IActionResult result = this._postController.Delete(Guid.Empty, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion
	}
}
