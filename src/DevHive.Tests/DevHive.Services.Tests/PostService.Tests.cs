using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models.Post.Comment;
using DevHive.Services.Models.Post.Post;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class PostServiceTests
	{
		private const string MESSAGE = "Gosho Trapov";
		private Mock<IPostRepository> PostRepositoryMock { get; set; }
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private PostService PostService { get; set; }

		[SetUp]
		public void Setup()
		{
			this.PostRepositoryMock = new Mock<IPostRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.PostService = new PostService(this.PostRepositoryMock.Object, this.UserRepositoryMock.Object, this.MapperMock.Object);
		}

		#region Comment
		#region Create
		[Test]
		public async Task AddComment_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};
			Comment comment = new Comment
			{
				Message = MESSAGE,
				Id = id
			};

			this.PostRepositoryMock.Setup(p => p.AddCommentAsync(It.IsAny<Comment>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetCommentByIssuerAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(comment));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this.PostService.AddComment(createCommentServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task CreateLanguage_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			CreateCommentServiceModel createCommentServiceModel = new CreateCommentServiceModel
			{
				Message = MESSAGE
			};
			Comment comment = new Comment
			{
				Message = MESSAGE,
			};

			this.PostRepositoryMock.Setup(p => p.AddCommentAsync(It.IsAny<Comment>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this.PostService.AddComment(createCommentServiceModel);

			Assert.IsTrue(result == Guid.Empty);
		}
		#endregion

		#region Read
		[Test]
		public async Task GetCommentById_ReturnsTheComment_WhenItExists()
		{
			Guid id = new Guid();
			Comment comment = new Comment
			{
				Message = MESSAGE
			};
			CommentServiceModel commentServiceModel = new CommentServiceModel
			{
				Message = MESSAGE
			};

			this.PostRepositoryMock.Setup(p => p.GetCommentByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			this.MapperMock.Setup(p => p.Map<CommentServiceModel>(It.IsAny<Comment>())).Returns(commentServiceModel);

			CommentServiceModel result = await this.PostService.GetCommentById(id);

			Assert.AreEqual(MESSAGE, result.Message);
		}

		[Test]
		public void GetLanguageById_ThrowsException_WhenLanguageDoesNotExist()
		{
			string exceptionMessage = "The comment does not exist";
			Guid id = new Guid();
			this.PostRepositoryMock.Setup(p => p.GetCommentByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Comment>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.GetCommentById(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region Update
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task UpdateComment_ReturnsIfUpdateIsSuccessfull_WhenCommentExistsy(bool shouldPass)
		{
			Comment comment = new Comment
			{
				Message = MESSAGE
			};
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
				Message = MESSAGE
			};

			this.PostRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.EditCommentAsync(It.IsAny<Comment>())).Returns(Task.FromResult(shouldPass));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<UpdateCommentServiceModel>())).Returns(comment);

			bool result = await this.PostService.UpdateComment(updateCommentServiceModel);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void UpdateLanguage_ThrowsArgumentException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			UpdateCommentServiceModel updateCommentServiceModel = new UpdateCommentServiceModel
			{
			};

			this.PostRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.UpdateComment(updateCommentServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region Delete
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteComment_ShouldReturnIfDeletionIsSuccessfull_WhenCommentExists(bool shouldPass)
		{
			Guid id = new Guid();
			Comment comment = new Comment();

			this.PostRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetCommentByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));
			this.PostRepositoryMock.Setup(p => p.DeleteCommentAsync(It.IsAny<Comment>())).Returns(Task.FromResult(shouldPass));

			bool result = await this.PostService.DeleteComment(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteLanguage_ThrowsException_WhenLanguageDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			Guid id = new Guid();

			this.PostRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.DeleteComment(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region ValidateJwtForComment
		//TO DO: Implement
		#endregion
		#endregion

		#region Posts
		#region Create
		[Test]
		public async Task CreatePost_ReturnsIdOfThePost_WhenItIsSuccessfullyCreated()
		{
			Guid id = Guid.NewGuid();
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
			};
			Post post = new Post
			{
				Message = MESSAGE,
				Id = id
			};

			this.PostRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Post>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetPostByIssuerAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(post));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.CreatePost(createPostServiceModel);

			Assert.AreEqual(id, result, "CreatePost does not return the correct id");
		}

		[Test]
		public async Task CreatePost_ReturnsEmptyGuid_WhenItIsNotSuccessfullyCreated()
		{
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
			};
			Post post = new Post
			{
				Message = MESSAGE,
			};

			this.PostRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Post>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.CreatePost(createPostServiceModel);

			Assert.AreEqual(Guid.Empty, result, "CreatePost does not return empty id");
		}
		#endregion


		#endregion
	}
}
