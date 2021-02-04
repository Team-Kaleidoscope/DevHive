using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
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
		private Mock<ICloudService> CloudServiceMock { get; set; }
		private Mock<IPostRepository> PostRepositoryMock { get; set; }
		private Mock<ICommentRepository> CommentRepositoryMock { get; set; }
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private PostService PostService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.PostRepositoryMock = new Mock<IPostRepository>();
			this.CloudServiceMock = new Mock<ICloudService>();
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.CommentRepositoryMock = new Mock<ICommentRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.PostService = new PostService(this.CloudServiceMock.Object, this.UserRepositoryMock.Object, this.PostRepositoryMock.Object, this.CommentRepositoryMock.Object, this.MapperMock.Object);
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

			this.PostRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Post>())).Returns(Task.FromResult(true));
			this.PostRepositoryMock.Setup(p => p.GetPostByCreatorAndTimeCreatedAsync(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(Task.FromResult(post));
			this.UserRepositoryMock.Setup(p => p.DoesUserExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(creator));
			this.MapperMock.Setup(p => p.Map<Post>(It.IsAny<CreatePostServiceModel>())).Returns(post);

			Guid result = await this.PostService.CreatePost(createPostServiceModel);

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
			User creator = new User { Id = creatorId };
			Post post = new Post
			{
				Message = MESSAGE,
				Creator = creator
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
			User creator = new User { Id = creatorId };
			Post post = new Post
			{
				Message = MESSAGE,
				Creator = creator
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
				NewMessage = MESSAGE,
				Files = new List<IFormFile>()
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
				NewMessage = MESSAGE,
				Files = new List<IFormFile>()
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
	}
}
