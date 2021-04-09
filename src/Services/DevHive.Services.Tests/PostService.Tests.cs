using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Post;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class PostServiceTests
	{
		private const string MESSAGE = "Gosho Trapov";
		private Mock<ICloudService> _cloudServiceMock;
		private Mock<IPostRepository> _postRepositoryMock;
		private Mock<ICommentRepository> _commentRepositoryMock;
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<IMapper> _mapperMock;
		private PostService _postService;

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this._postRepositoryMock = new Mock<IPostRepository>();
			this._cloudServiceMock = new Mock<ICloudService>();
			this._userRepositoryMock = new Mock<IUserRepository>();
			this._commentRepositoryMock = new Mock<ICommentRepository>();
			this._mapperMock = new Mock<IMapper>();
			this._postService = new PostService(this._cloudServiceMock.Object, this._userRepositoryMock.Object, this._postRepositoryMock.Object, this._commentRepositoryMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region CreatePost
		[Test]
		public async Task CreatePost_ReturnsIdOfThePost_WhenItIsSuccessfullyCreated()
		{
			Guid postId = Guid.NewGuid();
			User creator = new User { Id = Guid.NewGuid() };
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
				Files = new List<IFormFile>()
			};
			Post post = new Post
			{
				Message = MESSAGE,
				Id = postId,
			};

			this._postRepositoryMock
				.Setup(p => p.AddAsync(It.IsAny<Post>()))
				.ReturnsAsync(true);
			this._postRepositoryMock
				.Setup(p => p.GetPostByCreatorAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>()))
				.ReturnsAsync(post);
			this._userRepositoryMock
				.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(creator);
			this._mapperMock
				.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>()))
				.Returns(post);

			Guid result = await this._postService.CreatePost(createPostServiceModel);

			Assert.AreEqual(postId, result, "CreatePost does not return the correct id");
		}

		[Test]
		public async Task CreatePost_ReturnsEmptyGuid_WhenItIsNotSuccessfullyCreated()
		{
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
				Files = new List<IFormFile>()
			};
			Post post = new Post
			{
				Message = MESSAGE,
			};

			this._postRepositoryMock
				.Setup(p => p.AddAsync(It.IsAny<Post>()))
				.ReturnsAsync(false);
			this._userRepositoryMock
				.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._mapperMock
				.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>()))
				.Returns(post);

			Guid result = await this._postService.CreatePost(createPostServiceModel);

			Assert.AreEqual(Guid.Empty, result, "CreatePost does not return empty id");
		}

		[Test]
		public void CreatePost_ThrowsException_WhenUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "User does not exist!";
			CreatePostServiceModel createPostServiceModel = new CreatePostServiceModel
			{
			};

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._postService.CreatePost(createPostServiceModel), "CreatePost does not throw excpeion when the user does not exist");

			// Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Excapetion message is not correct");
		}
		#endregion

		#region GetPostById
		[Test]
		public async Task GetPostById_ReturnsThePost_WhenItExists()
		{
			Guid creatorId = Guid.NewGuid();
			User creator = new User { Id = creatorId };
			Post post = new Post
			{
				Message = MESSAGE,
				Creator = creator,
				Ratings = new List<Rating>()
			};
			ReadPostServiceModel readPostServiceModel = new ReadPostServiceModel
			{
				Message = MESSAGE
			};
			User user = new User
			{
				Id = creatorId,
			};

			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(post);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);
			this._mapperMock
				.Setup(p => p.Map<ReadPostServiceModel>(It.IsAny<Post>()))
				.Returns(readPostServiceModel);

			ReadPostServiceModel result = await this._postService.GetPostById(Guid.NewGuid());

			Assert.AreEqual(MESSAGE, result.Message);
		}

		[Test]
		public void GetPostById_ThorwsException_WhenTheUserDoesNotExist()
		{
			const string EXCEPTION_MESSAGE = "The user does not exist!";
			Guid creatorId = Guid.NewGuid();
			User creator = new User { Id = creatorId };
			Post post = new Post
			{
				Message = MESSAGE,
				Creator = creator
			};

			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(post);

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._postService.GetPostById(Guid.NewGuid()), "GetPostById does not throw exception when the user does not exist");

			// Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message);
		}

		[Test]
		public void GetPostById_ThrowsException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "The post does not exist!";
			Guid creatorId = Guid.NewGuid();
			User user = new User
			{
				Id = creatorId,
			};

			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult<Post>(null));
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._postService.GetPostById(Guid.NewGuid()));

			// Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
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
				NewMessage = MESSAGE,
				Files = new List<IFormFile>()
			};

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._postRepositoryMock
				.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Post>()))
				.ReturnsAsync(true);
			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(post);
			this._mapperMock
				.Setup(p => p.Map<Post>(It.IsAny<UpdatePostServiceModel>()))
				.Returns(post);

			Guid result = await this._postService.UpdatePost(updatePostServiceModel);

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
				NewMessage = MESSAGE,
				Files = new List<IFormFile>()
			};

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._postRepositoryMock
				.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Post>()))
				.ReturnsAsync(false);
			this._mapperMock
				.Setup(p => p.Map<Post>(It.IsAny<UpdatePostServiceModel>()))
				.Returns(post);

			Guid result = await this._postService.UpdatePost(updatePostServiceModel);

			Assert.AreEqual(Guid.Empty, result);
		}

		[Test]
		public void UpdatePost_ThrowsArgumentException_WhenCommentDoesNotExist()
		{
			string exceptionMessage = "Post does not exist!";
			UpdatePostServiceModel updatePostServiceModel = new UpdatePostServiceModel
			{
			};

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(false);

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._postService.UpdatePost(updatePostServiceModel));

			// Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeletePost
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task Deletepost_ShouldReturnIfDeletionIsSuccessfull_WhenPostExists(bool shouldPass)
		{
			Guid id = Guid.NewGuid();
			Post post = new Post();

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(post);
			this._postRepositoryMock
				.Setup(p => p.DeleteAsync(It.IsAny<Post>()))
				.ReturnsAsync(shouldPass);

			bool result = await this._postService.DeletePost(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeletePost_ThrowsException_WhenPostDoesNotExist()
		{
			string exceptionMessage = "Post does not exist!";
			Guid id = Guid.NewGuid();

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(false);

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._postService.DeletePost(id));

			// Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
