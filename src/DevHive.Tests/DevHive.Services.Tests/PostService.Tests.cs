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
		private Mock<ICommentRepository> CommentRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private PostService PostService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.PostRepositoryMock = new Mock<IPostRepository>();
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.CommentRepositoryMock = new Mock<ICommentRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.PostService = new PostService(this.UserRepositoryMock.Object, this.PostRepositoryMock.Object, this.CommentRepositoryMock.Object, this.MapperMock.Object);
		}
		#endregion

		#region Comment
		#region AddComment
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

			this.CommentRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Comment>())).Returns(Task.FromResult(true));
			this.CommentRepositoryMock.Setup(p => p.GetCommentByIssuerAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(comment));
			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<Comment>(It.IsAny<CreateCommentServiceModel>())).Returns(comment);

			Guid result = await this.PostService.AddComment(createCommentServiceModel);

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

			Guid result = await this.PostService.AddComment(createCommentServiceModel);

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

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.AddComment(createCommentServiceModel), "AddComment does not throw excpeion when the post does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetCommentById
		[Test]
		public async Task GetCommentById_ReturnsTheComment_WhenItExists()
		{
			Guid creatorId = new Guid();
			Comment comment = new Comment
			{
				Message = MESSAGE,
				CreatorId = creatorId
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

			ReadCommentServiceModel result = await this.PostService.GetCommentById(new Guid());

			Assert.AreEqual(MESSAGE, result.Message);
		}

		[Test]
		public void GetCommentById_ThorwsException_WhenTheUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "The user does not exist";
			Guid creatorId = new Guid();
			Comment comment = new Comment
			{
				Message = MESSAGE,
				CreatorId = creatorId
			};

			this.CommentRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(comment));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.GetCommentById(new Guid()), "GetCommentById does not throw exception when the user does not exist");

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

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.GetCommentById(new Guid()));

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

			Guid result = await this.PostService.UpdateComment(updateCommentServiceModel);

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

			Guid result = await this.PostService.UpdateComment(updateCommentServiceModel);

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

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.UpdateComment(updateCommentServiceModel));

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

			bool result = await this.PostService.DeleteComment(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteComment_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Comment does not exist!";
			Guid id = new Guid();

			this.CommentRepositoryMock.Setup(p => p.DoesCommentExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.DeleteComment(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region ValidateJwtForComment
		//TO DO: Implement
		#endregion
		#endregion

		#region Posts
		#region CreatePost
		[Test]
		public async Task CreatePost_ReturnsIdOfThePost_WhenItIsSuccessfullyCreated()
		{
			Guid postId = Guid.NewGuid();
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
			};
			Post post = new Post
			{
				Message = MESSAGE,
				Id = postId
			};

			this.PostRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Post>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetPostByCreatorAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(post));
			this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.CreatePost(createPostServiceModel);

			Assert.AreEqual(postId, result, "CreatePost does not return the correct id");
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
			this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.CreatePost(createPostServiceModel);

			Assert.AreEqual(Guid.Empty, result, "CreatePost does not return empty id");
		}

		[Test]
		public void CreatePost_ThrowsException_WhenUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "User does not exist!";
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
			};
			Post post = new Post
			{
				Message = MESSAGE,
			};

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.CreatePost(createPostServiceModel), "CreatePost does not throw excpeion when the user does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Excapetion message is not correct");
		}
		#endregion

		#region GetPostById
		[Test]
		public async Task GetPostById_ReturnsThePost_WhenItExists()
		{
			Guid creatorId = new Guid();
			Post post = new Post
			{
				Message = MESSAGE,
				CreatorId = creatorId
			};
			ReadPostServiceModel readPostServiceModel = new ReadPostServiceModel
			{
				Message = MESSAGE
			};
			User user = new User
			{
				Id = creatorId,
			};

			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(post));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.MapperMock.Setup(p => p.Map<ReadPostServiceModel>(It.IsAny<Post>())).Returns(readPostServiceModel);

			ReadPostServiceModel result = await this.PostService.GetPostById(new Guid());

			Assert.AreEqual(MESSAGE, result.Message);
		}

		[Test]
		public void GetPostById_ThorwsException_WhenTheUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "The user does not exist!";
			Guid creatorId = new Guid();
			Post post = new Post
			{
				Message = MESSAGE,
				CreatorId = creatorId
			};

			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(post));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.GetPostById(new Guid()), "GetPostById does not throw exception when the user does not exist");

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message);
		}

		[Test]
		public void GetPostById_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "The post does not exist!";
			Guid creatorId = new Guid();
			User user = new User
			{
				Id = creatorId,
			};

			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Post>(null));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.GetPostById(new Guid()));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region UpdatePost
		[Test]
		public async Task UpdatePost_ReturnsTheIdOfThePost_WhenUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			Post post = new Post
			{
				Id = id,
				Message = MESSAGE
			};
			UpdatePostServiceModel updatePostServiceModel = new UpdatePostServiceModel
			{
				PostId = id,
				NewMessage = MESSAGE
			};

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Post>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(post));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<UpdatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.UpdatePost(updatePostServiceModel);

			Assert.AreEqual(updatePostServiceModel.PostId, result);
		}

		[Test]
		public async Task UpdatePost_ReturnsEmptyId_WhenThePostIsNotUpdatedSuccessfully()
		{
			Post post = new Post
			{
				Message = MESSAGE
			};
			UpdatePostServiceModel updatePostServiceModel = new UpdatePostServiceModel
			{
				PostId = Guid.NewGuid(),
				NewMessage = MESSAGE
			};

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Post>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<UpdatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.UpdatePost(updatePostServiceModel);

			Assert.AreEqual(Guid.Empty, result);
		}

		[Test]
		public void UpdatePost_ThrowsArgumentException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Post does not exist!";
			UpdatePostServiceModel updatePostServiceModel = new UpdatePostServiceModel
			{
			};

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.UpdatePost(updatePostServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeletePost
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task Deletepost_ShouldReturnIfDeletionIsSuccessfull_WhenPostExists(bool shouldPass)
		{
			Guid id = new Guid();
			Post post = new Post();

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(post));
			this.PostRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Post>())).Returns(Task.FromResult(shouldPass));

			bool result = await this.PostService.DeletePost(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeletePost_ThrowsException_WhenPostDoesNotExist()
		{
			string exceptionMessage = "Post does not exist!";
			Guid id = new Guid();

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.PostService.DeletePost(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region ValidateJwtForPost
		//TO DO: Implement
		#endregion
		#endregion
	}
}
