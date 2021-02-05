using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models.Comment;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class CommentServiceTests
	{
		private const string MESSAGE = "Gosho Trapov";
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IPostRepository> PostRepositoryMock { get; set; }
		private Mock<ICommentRepository> CommentRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private CommentService CommentService { get; set; }

		#region Setup
		[SetUp]
		public void Setup()
		{
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.PostRepositoryMock = new Mock<IPostRepository>();
			this.CommentRepositoryMock = new Mock<ICommentRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.CommentService = new CommentService(this.UserRepositoryMock.Object, this.PostRepositoryMock.Object, this.CommentRepositoryMock.Object, this.MapperMock.Object);
		}
		#endregion

		#region AddComment
		[Test]
		public async Task AddComment_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			User creator = new User { Id = Guid.NewGuid() };
			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};
			Comment comment = new Comment
			{
				Message = MESSAGE,
				Id = id,
			};

			this.CommentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Comment>())).Returns(Task.FromResult(true));
			this.CommentRepositoryMock.Setup(p => p.GetCommentByIssuerAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(comment));
			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(creator));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this.CommentService.AddComment(createCommentServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task AddComment_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};
			Comment comment = new Comment
			{
				Message = MESSAGE,
			};

			this.CommentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Comment>())).Returns(Task.FromResult(false));
			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this.CommentService.AddComment(createCommentServiceModel);

			Assert.IsTrue(result == Guid.Empty);
		}

		[Test]
		public void AddComment_ThrowsException_WhenPostDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "Post does not exist!";

			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.CommentService.AddComment(createCommentServiceModel), "AddComment does not throw excpeion when the post does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetCommentById
		[Test]
		public async Task GetCommentById_ReturnsTheComment_WhenItExists()
		{
			Guid creatorId = new Guid();
			User creator = new User { Id = creatorId };
			Comment comment = new Comment
			{
				Message = MESSAGE,
				Creator = creator
			};
			ReadCommentServiceModel commentServiceModel = new ReadCommentServiceModel
			{
				Message = MESSAGE
			};
			User user = new User
			{
				Id = creatorId,
			};

			this.CommentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.MapperMock.Setup(p => p.Map<ReadCommentServiceModel>(It.IsAny<Comment>())).Returns(commentServiceModel);

			ReadCommentServiceModel result = await this.CommentService.GetCommentById(new Guid());

			Assert.AreEqual(MESSAGE, result.Message);
		}

		[Test]
		public void GetCommentById_ThorwsException_WhenTheUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "The user does not exist";
			Guid creatorId = new Guid();
			User creator = new User { Id = creatorId };
			Comment comment = new Comment
			{
				Message = MESSAGE,
				Creator = creator
			};

			this.CommentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.CommentService.GetCommentById(new Guid()), "GetCommentById does not throw exception when the user does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message);
		}

		[Test]
		public void GetCommentById_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "The comment does not exist";
			Guid creatorId = new Guid();
			User user = new User
			{
				Id = creatorId,
			};

			this.CommentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Comment>(null));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.CommentService.GetCommentById(new Guid()));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region UpdateComment
		[Test]
		public async Task UpdateComment_ReturnsTheIdOfTheComment_WhenUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			Comment comment = new Comment
			{
				Id = id,
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
				CommentId = id,
				NewMessage = MESSAGE
			};

			this.CommentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.CommentRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Comment>())).Returns(Task.FromResult(true));
			this.CommentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<UpdateCommentServiceModel>())).Returns(comment);

			Guid result = await this.CommentService.UpdateComment(updateCommentServiceModel);

			Assert.AreEqual(updateCommentServiceModel.CommentId, result);
		}

		[Test]
		public async Task UpdateComment_ReturnsEmptyId_WhenTheCommentIsNotUpdatedSuccessfully()
		{
			Comment comment = new Comment
			{
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
				CommentId = Guid.NewGuid(),
				NewMessage = MESSAGE
			};

			this.CommentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.CommentRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Comment>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<UpdateCommentServiceModel>())).Returns(comment);

			Guid result = await this.CommentService.UpdateComment(updateCommentServiceModel);

			Assert.AreEqual(Guid.Empty, result);
		}

		[Test]
		public void UpdateComment_ThrowsArgumentException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
			};

			this.CommentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.CommentService.UpdateComment(updateCommentServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteComment
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteComment_ShouldReturnIfDeletionIsSuccessfull_WhenCommentExists(bool shouldPass)
		{
			Guid id = new Guid();
			Comment comment = new Comment();

			this.CommentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.CommentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			this.CommentRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Comment>())).Returns(Task.FromResult(shouldPass));

			bool result = await this.CommentService.DeleteComment(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteComment_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			Guid id = new Guid();

			this.CommentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.CommentService.DeleteComment(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}