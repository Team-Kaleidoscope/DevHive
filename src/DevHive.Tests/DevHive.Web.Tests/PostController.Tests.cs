using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Post.Comment;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Post.Comment;
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

		#region Comment
		#region Create
		[Test]
		public void AddComment_ReturnsOkObjectResult_WhenCommentIsSuccessfullyCreated()
		{
			CommentWebModel commentWebModel = new CommentWebModel
			{
				Message = MESSAGE
			};
			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};
			Guid id = Guid.NewGuid();

			this.MapperMock.Setup(p => p.Map<CreateCommentServiceModel>(It.IsAny<CommentWebModel>())).Returns(createCommentServiceModel);
			this.PostServiceMock.Setup(p => p.AddComment(It.IsAny<CreateCommentServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.PostController.AddComment(commentWebModel).Result;

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
		public void AddComment_ReturnsBadRequestObjectResult_WhenCommentIsNotCreatedSuccessfully()
		{
			CommentWebModel commentWebModel = new CommentWebModel
			{
				Message = MESSAGE
			};
			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};
			Guid id = Guid.Empty;
			string errorMessage = $"Could not create comment";

			this.MapperMock.Setup(p => p.Map<CreateCommentServiceModel>(It.IsAny<CommentWebModel>())).Returns(createCommentServiceModel);
			this.PostServiceMock.Setup(p => p.AddComment(It.IsAny<CreateCommentServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.PostController.AddComment(commentWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequsetObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequsetObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}
		#endregion

		#region Read
		[Test]
		public void GetCommentById_ReturnsTheComment_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			CommentServiceModel commentServiceModel = new CommentServiceModel
			{
				Message = MESSAGE
			};
			CommentWebModel commentWebModel = new CommentWebModel
			{
				Message = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.GetCommentById(It.IsAny<Guid>())).Returns(Task.FromResult(commentServiceModel));
			this.MapperMock.Setup(p => p.Map<CommentWebModel>(It.IsAny<CommentServiceModel>())).Returns(commentWebModel);

			IActionResult result = this.PostController.GetCommentById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			CommentWebModel resultModel = okObjectResult.Value as Models.Post.Comment.CommentWebModel;

			Assert.AreEqual(MESSAGE, resultModel.Message);
		}
		#endregion

		#region Update
		[Test]
		public void UpdateComment_ShouldReturnOkResult_WhenCommentIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			CommentWebModel commentWebModel = new CommentWebModel
			{
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
				Message = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.UpdateComment(It.IsAny<UpdateCommentServiceModel>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateCommentServiceModel>(It.IsAny<CommentWebModel>())).Returns(updateCommentServiceModel);
			this.PostServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.UpdateComment(id, commentWebModel, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void UpdateComment_ShouldReturnBadObjectResult_WhenCommentIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			string message = "Could not update Comment";
			CommentWebModel commentWebModel = new CommentWebModel
			{
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
				Message = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.UpdateComment(It.IsAny<UpdateCommentServiceModel>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<UpdateCommentServiceModel>(It.IsAny<CommentWebModel>())).Returns(updateCommentServiceModel);
			this.PostServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.UpdateComment(id, commentWebModel, null).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void UpdateComment_ShouldReturnUnauthorizedResult_WhenJwtIsNotValid()
		{
			Guid id = Guid.NewGuid();

			CommentWebModel commentWebModel = new CommentWebModel
			{
				Message = MESSAGE
			};

			this.PostServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.PostController.UpdateComment(id, commentWebModel, null).Result;
			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Delete
		[Test]
		public void DeleteComment_ReturnsOkResult_WhenLanguageIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this.PostServiceMock.Setup(p => p.DeleteComment(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.DeleteComment(id, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void DeletComment_ReturnsBadRequestObjectResult_WhenLanguageIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Comment";
			Guid id = Guid.NewGuid();

			this.PostServiceMock.Setup(p => p.DeleteComment(It.IsAny<Guid>())).Returns(Task.FromResult(false));
			this.PostServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.PostController.DeleteComment(id, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void DeletComment_ReturnsUnauthorizedResult_WhenJwtIsNotValid()
		{
			Guid id = Guid.NewGuid();

			this.PostServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.PostController.DeleteComment(id, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion
		#endregion
	}
}
