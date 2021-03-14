using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces;
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
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<IPostRepository> _postRepositoryMock;
		private Mock<ICommentRepository> _commentRepositoryMock;
		private Mock<IMapper> _mapperMock;
		private CommentService _commentService;

		#region Setup
		[SetUp]
		public void Setup()
		{
			this._userRepositoryMock = new Mock<IUserRepository>();
			this._postRepositoryMock = new Mock<IPostRepository>();
			this._commentRepositoryMock = new Mock<ICommentRepository>();
			this._mapperMock = new Mock<IMapper>();
			this._commentService = new CommentService(this._userRepositoryMock.Object, this._postRepositoryMock.Object, this._commentRepositoryMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region AddComment
		[Test]
		public async Task AddComment_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			User creator = new() { Id = Guid.NewGuid() };
			CreateCommentServiceModel createCommentServiceModel = new()
			{
				Message = MESSAGE
			};
			Comment comment = new()
			{
				Message = MESSAGE,
				Id = id,
			};

			_ = this._commentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Comment>())).Returns(Task.FromResult(true));
			_ = this._commentRepositoryMock.Setup(p => p.GetCommentByIssuerAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(comment));
			_ = this._postRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			_ = this._userRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(creator));
			_ = this._mapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this._commentService.AddComment(createCommentServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task AddComment_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			CreateCommentServiceModel createCommentServiceModel = new()
			{
				Message = MESSAGE
			};
			Comment comment = new()
			{
				Message = MESSAGE,
			};

			_ = this._commentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Comment>())).Returns(Task.FromResult(false));
			_ = this._postRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			_ = this._mapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this._commentService.AddComment(createCommentServiceModel);

			Assert.IsTrue(result == Guid.Empty);
		}

		[Test]
		public void AddComment_ThrowsException_WhenPostDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "Post does not exist!";

			CreateCommentServiceModel createCommentServiceModel = new()
			{
				Message = MESSAGE
			};

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._commentService.AddComment(createCommentServiceModel), "AddComment does not throw excpeion when the post does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetCommentById
		[Test]
		public async Task GetCommentById_ReturnsTheComment_WhenItExists()
		{
			Guid creatorId = new();
			User creator = new() { Id = creatorId };
			Comment comment = new()
			{
				Message = MESSAGE,
				Creator = creator
			};
			ReadCommentServiceModel commentServiceModel = new()
			{
				Message = MESSAGE
			};
			User user = new()
			{
				Id = creatorId,
			};

			_ = this._commentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			_ = this._userRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			_ = this._mapperMock.Setup(p => p.Map<ReadCommentServiceModel>(It.IsAny<Comment>())).Returns(commentServiceModel);

			ReadCommentServiceModel result = await this._commentService.GetCommentById(Guid.NewGuid());

			Assert.AreEqual(MESSAGE, result.Message);
		}

		[Test]
		public void GetCommentById_ThorwsException_WhenTheUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "The user does not exist";
			Guid creatorId = new();
			User creator = new() { Id = creatorId };
			Comment comment = new()
			{
				Message = MESSAGE,
				Creator = creator
			};

			_ = this._commentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._commentService.GetCommentById(Guid.NewGuid()), "GetCommentById does not throw exception when the user does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message);
		}

		[Test]
		public void GetCommentById_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "The comment does not exist";
			Guid creatorId = new();
			User user = new()
			{
				Id = creatorId,
			};

			_ = this._commentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Comment>(null));
			_ = this._userRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._commentService.GetCommentById(Guid.NewGuid()));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region UpdateComment
		[Test]
		public async Task UpdateComment_ReturnsTheIdOfTheComment_WhenUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			Comment comment = new()
			{
				Id = id,
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new()
			{
				CommentId = id,
				NewMessage = MESSAGE
			};

			_ = this._commentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			_ = this._commentRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Comment>())).Returns(Task.FromResult(true));
			_ = this._commentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			_ = this._mapperMock.Setup(p => p.Map<Comment>(It.IsAny<UpdateCommentServiceModel>())).Returns(comment);

			Guid result = await this._commentService.UpdateComment(updateCommentServiceModel);

			Assert.AreEqual(updateCommentServiceModel.CommentId, result);
		}

		[Test]
		public async Task UpdateComment_ReturnsEmptyId_WhenTheCommentIsNotUpdatedSuccessfully()
		{
			Comment comment = new()
			{
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new()
			{
				CommentId = Guid.NewGuid(),
				NewMessage = MESSAGE
			};

			_ = this._commentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			_ = this._commentRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Comment>())).Returns(Task.FromResult(false));
			_ = this._mapperMock.Setup(p => p.Map<Comment>(It.IsAny<UpdateCommentServiceModel>())).Returns(comment);

			Guid result = await this._commentService.UpdateComment(updateCommentServiceModel);

			Assert.AreEqual(Guid.Empty, result);
		}

		[Test]
		public void UpdateComment_ThrowsArgumentException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			UpdateCommentServiceModel updateCommentServiceModel = new()
			{
			};

			_ = this._commentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._commentService.UpdateComment(updateCommentServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteComment
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteComment_ShouldReturnIfDeletionIsSuccessfull_WhenCommentExists(bool shouldPass)
		{
			Guid id = new();
			Comment comment = new();

			_ = this._commentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			_ = this._commentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			_ = this._commentRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Comment>())).Returns(Task.FromResult(shouldPass));

			bool result = await this._commentService.DeleteComment(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteComment_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			Guid id = new();

			_ = this._commentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._commentService.DeleteComment(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
