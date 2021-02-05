using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Data.RelationModels;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
    [TestFixture]
	public class PostRepositoryTests
	{
		private const string POST_MESSAGE = "Post test message";

		private DevHiveContext Context { get; set; }

		private Mock<IUserRepository> UserRepository { get; set; }

		private PostRepository PostRepository { get; set; }

		#region Setups
		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this.Context = new DevHiveContext(optionsBuilder.Options);

			this.UserRepository = new Mock<IUserRepository>();

			PostRepository = new PostRepository(Context, this.UserRepository.Object);
		}

		[TearDown]
		public void TearDown()
		{
			this.Context.Database.EnsureDeleted();
		}
		#endregion

		#region AddNewPostToCreator
		// [Test]
		// public async Task AddNewPostToCreator_ReturnsTrue_WhenNewPostIsAddedToCreator()
		// {
		// 	Post post = await this.AddEntity();
		// 	User user = new User { Id = Guid.NewGuid() };
        //
		// 	this.UserRepository.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
        //
		// 	bool result = await this.PostRepository.AddNewPostToCreator(user.Id, post);
        //
		// 	Assert.IsTrue(result, "AddNewPostToCreator does not return true when Post Is Added To Creator successfully");
		// }
		#endregion

		#region GetByIdAsync
		[Test]
		public async Task GetByNameAsync_ReturnsTheCorrectPost_IfItExists()
		{
			Post post = await AddEntity();

			Post resultTechnology = await this.PostRepository.GetByIdAsync(post.Id);

			Assert.AreEqual(post.Id, resultTechnology.Id, "GetByIdAsync does not return the correct post");
		}

		[Test]
		public async Task GetByIdAsync_ReturnsNull_IfTechnologyDoesNotExists()
		{
			Post resultPost = await this.PostRepository.GetByIdAsync(Guid.NewGuid());

			Assert.IsNull(resultPost);
		}
		#endregion

		#region GetPostByCreatorAndTimeCreatedAsync
		[Test]
		public async Task GetPostByCreatorAndTimeCreatedAsync_ReturnsTheCorrectPost_IfItExists()
		{
			Post post = await this.AddEntity();

			Post resultPost = await this.PostRepository.GetPostByCreatorAndTimeCreatedAsync(post.Creator.Id, post.TimeCreated);

			Assert.AreEqual(post.Id, resultPost.Id, "GetPostByCreatorAndTimeCreatedAsync does not return the corect post when it exists");
		}

		[Test]
		public async Task GetPostByCreatorAndTimeCreatedAsync_ReturnsNull_IfThePostDoesNotExist()
		{
			Post post = await this.AddEntity();

			Post resutPost = await this.PostRepository.GetPostByCreatorAndTimeCreatedAsync(Guid.Empty, DateTime.Now);

			Assert.IsNull(resutPost, "GetPostByCreatorAndTimeCreatedAsync does not return null when the post does not exist");
		}
		#endregion

		#region DoesPostExist
		[Test]
		public async Task DoesPostExist_ReturnsTrue_WhenThePostExists()
		{
			Post post = await this.AddEntity();

			bool result = await this.PostRepository.DoesPostExist(post.Id);

			Assert.IsTrue(result, "DoesPostExist does not return true whenm the Post exists");
		}

		[Test]
		public async Task DoesPostExist_ReturnsFalse_WhenThePostDoesNotExist()
		{
			bool result = await this.PostRepository.DoesPostExist(Guid.Empty);

			Assert.IsFalse(result, "DoesPostExist does not return false whenm the Post does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task<Post> AddEntity(string name = POST_MESSAGE)
		{
			User creator = new User { Id = Guid.NewGuid() };
			await this.Context.Users.AddAsync(creator);
			Post post = new Post
			{
				Message = POST_MESSAGE,
				Id = Guid.NewGuid(),
				Creator = creator,
				TimeCreated = DateTime.Now,
				Attachments = new List<PostAttachments> { new PostAttachments { FileUrl = "kur" }, new PostAttachments { FileUrl = "za" }, new PostAttachments { FileUrl = "tva" } },
				Comments = new List<Comment>()
			};

			await this.Context.Posts.AddAsync(post);
			await this.Context.SaveChangesAsync();

			return post;
		}
		#endregion
	}
}
