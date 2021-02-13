using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
		private Mock<IPostService> PostServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private PostController PostController { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.PostServiceMock = new Mock<IPostService>();
			this.MapperMock = new Mock<IMapper>();
			this.PostController = new PostController(this.PostServiceMock.Object, this.MapperMock.Object);
		}

		#region Create
		[Test]
		public void CreatePost_ReturnsOkObjectResult_WhenPostIsSuccessfullyCreated()
		{
			CreatePostWebModel createPostWebModel = new CreatePostWebModel
			{
				Message = MESSAGE
			};
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
				Message = MESSAGE
			};
			Guid id = Guid.NewGuid();

			this.MapperMock.Setup(p => p.Map<CreatePostServiceModel>(It.IsAny<CreatePostWebModel>())).Returns(createPostServiceModel);
			this.PostServiceMock.Setup(p => p.CreatePost(It.IsAny<CreatePostServiceModel>())).Returns(Task.FromResult(id));
			this.PostServiceMock.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.Create(Guid.Empty, createPostWebModel, null).Result;

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
			CreatePostWebModel createTechnologyWebModel = new CreatePostWebModel
			{
				Message = MESSAGE
			};
			CreatePostServiceModel createTechnologyServiceModel = new CreatePostServiceModel
			{
				Message = MESSAGE
			};
			Guid id = Guid.Empty;
			string errorMessage = $"Could not create post!";

			this.MapperMock.Setup(p => p.Map<CreatePostServiceModel>(It.IsAny<CreatePostWebModel>())).Returns(createTechnologyServiceModel);
			this.PostServiceMock.Setup(p => p.CreatePost(It.IsAny<CreatePostServiceModel>())).Returns(Task.FromResult(id));
			this.PostServiceMock.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.Create(Guid.Empty, createTechnologyWebModel, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequsetObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequsetObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}

		[Test]
		public void CreatePost_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			Guid id = Guid.NewGuid();
			CreatePostWebModel createPostWebModel = new CreatePostWebModel
			{
				Message = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.PostController.Create(Guid.NewGuid(), createPostWebModel, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsThePost_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			ReadPostServiceModel readPostServiceModel = new ReadPostServiceModel
			{
				Message = MESSAGE
			};
			ReadPostWebModel readPostWebModel = new ReadPostWebModel
			{
				Message = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.GetPostById(It.IsAny<Guid>())).Returns(Task.FromResult(readPostServiceModel));
			this.MapperMock.Setup(p => p.Map<ReadPostWebModel>(It.IsAny<ReadPostServiceModel>())).Returns(readPostWebModel);

			IActionResult result = this.PostController.GetById(id).Result;

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
			UpdatePostWebModel updatePostWebModel = new UpdatePostWebModel
			{
				NewMessage = MESSAGE
			};
			UpdatePostServiceModel updatePostServiceModel = new UpdatePostServiceModel
			{
				NewMessage = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.UpdatePost(It.IsAny<UpdatePostServiceModel>())).Returns(Task.FromResult(id));
			this.MapperMock.Setup(p => p.Map<UpdatePostServiceModel>(It.IsAny<UpdatePostWebModel>())).Returns(updatePostServiceModel);
			this.PostServiceMock.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.Update(id, updatePostWebModel, null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenPostIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			string message = "Could not update post!";
			UpdatePostWebModel updatePostWebModel = new UpdatePostWebModel
			{
				NewMessage = MESSAGE
			};
			UpdatePostServiceModel updatePostServiceModel = new UpdatePostServiceModel
			{
				NewMessage = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.UpdatePost(It.IsAny<UpdatePostServiceModel>())).Returns(Task.FromResult(Guid.Empty));
			this.MapperMock.Setup(p => p.Map<UpdatePostServiceModel>(It.IsAny<UpdatePostWebModel>())).Returns(updatePostServiceModel);
			this.PostServiceMock.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.Update(id, updatePostWebModel, null).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void Update_ShouldReturnUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			UpdatePostWebModel updatePostWebModel = new UpdatePostWebModel
			{
				NewMessage = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.PostController.Update(Guid.Empty, updatePostWebModel, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenPostIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this.PostServiceMock.Setup(p => p.DeletePost(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostServiceMock.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.Delete(id, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delete_ReturnsBadRequestObjectResult_WhenPostIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Post";
			Guid id = Guid.NewGuid();

			this.PostServiceMock.Setup(p => p.DeletePost(It.IsAny<Guid>())).Returns(Task.FromResult(false));
			this.PostServiceMock.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.Delete(id, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void DeletePost_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			this.PostServiceMock.Setup(p => p.ValidateJwtForPost(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.PostController.Delete(Guid.Empty, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion
	}
}
