using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Comment;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Comment;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
    [TestFixture]
	public class CommentControllerTests
	{
		const string MESSAGE = "Gosho Trapov";
		private Mock<ICommentService> CommentServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private Mock<IJwtService> JwtServiceMock { get; set; }
		private CommentController CommentController { get; set; }

		#region Setup
		[SetUp]
		public void SetUp()
		{
			this.CommentServiceMock = new Mock<ICommentService>();
			this.MapperMock = new Mock<IMapper>();
			this.JwtServiceMock = new Mock<IJwtService>();
			this.CommentController = new CommentController(this.CommentServiceMock.Object, this.MapperMock.Object, this.JwtServiceMock.Object);
		}
		#endregion

		#region Add
		[Test]
		public void AddComment_ReturnsOkObjectResult_WhenCommentIsSuccessfullyCreated()
		{
			Guid id = Guid.NewGuid();
			CreateCommentWebModel createCommentWebModel = new()
			{
				Message = MESSAGE
			};
			CreateCommentServiceModel createCommentServiceModel = new()
			{
				Message = MESSAGE
			};

			this.MapperMock.Setup(p => p.Map<CreateCommentServiceModel>(It.IsAny<CreateCommentWebModel>())).Returns(createCommentServiceModel);
			this.CommentServiceMock.Setup(p => p.AddComment(It.IsAny<CreateCommentServiceModel>())).Returns(Task.FromResult(id));
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.CommentController.AddComment(Guid.NewGuid(), createCommentWebModel, null).Result;

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
			CreateCommentWebModel createCommentWebModel = new()
			{
				Message = MESSAGE
			};
			CreateCommentServiceModel createCommentServiceModel = new()
			{
				Message = MESSAGE
			};
			string errorMessage = $"Could not create comment!";


			this.MapperMock.Setup(p => p.Map<CreateCommentServiceModel>(It.IsAny<CreateCommentWebModel>())).Returns(createCommentServiceModel);
			this.CommentServiceMock.Setup(p => p.AddComment(It.IsAny<CreateCommentServiceModel>())).Returns(Task.FromResult(Guid.Empty));
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.CommentController.AddComment(Guid.NewGuid(), createCommentWebModel, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}

		[Test]
		public void AddComment_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			CreateCommentWebModel createCommentWebModel = new()
			{
				Message = MESSAGE
			};

			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForCreating(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.CommentController.AddComment(Guid.NewGuid(), createCommentWebModel, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsTheComment_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			ReadCommentServiceModel readCommentServiceModel = new()
			{
				Message = MESSAGE
			};
			ReadCommentWebModel readCommentWebModel = new()
			{
				Message = MESSAGE
			};

			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.GetCommentById(It.IsAny<Guid>())).Returns(Task.FromResult(readCommentServiceModel));
			this.MapperMock.Setup(p => p.Map<ReadCommentWebModel>(It.IsAny<ReadCommentServiceModel>())).Returns(readCommentWebModel);

			IActionResult result = this.CommentController.GetCommentById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadCommentWebModel resultModel = okObjectResult.Value as ReadCommentWebModel;

			Assert.AreEqual(MESSAGE, resultModel.Message);
		}
		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenCommentIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdateCommentWebModel updateCommentWebModel = new()
			{
				NewMessage = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new()
			{
				NewMessage = MESSAGE
			};

			this.CommentServiceMock.Setup(p => p.UpdateComment(It.IsAny<UpdateCommentServiceModel>())).Returns(Task.FromResult(id));
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateCommentServiceModel>(It.IsAny<UpdateCommentWebModel>())).Returns(updateCommentServiceModel);

			IActionResult result = this.CommentController.UpdateComment(Guid.Empty, updateCommentWebModel, null).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			object resultModel = okObjectResult.Value;
			string[] resultAsString = resultModel.ToString().Split(' ').ToArray();

			Assert.AreEqual(id.ToString(), resultAsString[3]);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenCommentIsNotUpdatedSuccessfully()
		{
			string message = "Unable to update comment!";
			UpdateCommentWebModel updateCommentWebModel = new()
			{
				NewMessage = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new()
			{
				NewMessage = MESSAGE
			};

			this.CommentServiceMock.Setup(p => p.UpdateComment(It.IsAny<UpdateCommentServiceModel>())).Returns(Task.FromResult(Guid.Empty));
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateCommentServiceModel>(It.IsAny<UpdateCommentWebModel>())).Returns(updateCommentServiceModel);

			IActionResult result = this.CommentController.UpdateComment(Guid.Empty, updateCommentWebModel, null).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void Update_ShouldReturnUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			UpdateCommentWebModel updateCommentWebModel = new()
			{
				NewMessage = MESSAGE
			};

			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(false);
			// this.CommentServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.CommentController.UpdateComment(Guid.Empty, updateCommentWebModel, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenCommentIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this.CommentServiceMock.Setup(p => p.DeleteComment(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.CommentController.DeleteComment(id, null).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void DeleteComment_ReturnsBadRequestObjectResult_WhenCommentIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Comment";
			Guid id = Guid.NewGuid();

			this.CommentServiceMock.Setup(p => p.DeleteComment(It.IsAny<Guid>())).Returns(Task.FromResult(false));
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(true));

			IActionResult result = this.CommentController.DeleteComment(id, null).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}

		[Test]
		public void DeleteComment_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
		{
			this.JwtServiceMock.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>())).Returns(true);
			this.CommentServiceMock.Setup(p => p.ValidateJwtForComment(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(false));

			IActionResult result = this.CommentController.DeleteComment(Guid.Empty, null).Result;

			Assert.IsInstanceOf<UnauthorizedResult>(result);
		}
		#endregion
	}
}
