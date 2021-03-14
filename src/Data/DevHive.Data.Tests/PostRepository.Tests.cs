using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Data.Models.Relational;
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
		private DevHiveContext _context;
		private Mock<IUserRepository> _userRepository;
		private PostRepository _postRepository;

		#region Setups
		[SetUp]
		public void Setup()
		{
			DbContextOptionsBuilder<DevHiveContext> optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this._context = new DevHiveContext(optionsBuilder.Options);

			this._userRepository = new Mock<IUserRepository>();

			this._postRepository = new PostRepository(this._context, this._userRepository.Object);
		}

		[TearDown]
		public void TearDown()
		{
			this._context.Database.EnsureDeleted();
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
			Post post = await this.AddEntity();

			Post resultTechnology = await this._postRepository.GetByIdAsync(post.Id);

			Assert.AreEqual(post.Id, resultTechnology.Id, "GetByIdAsync does not return the correct post");
		}

		[Test]
		public async Task GetByIdAsync_ReturnsNull_IfTechnologyDoesNotExists()
		{
			Post resultPost = await this._postRepository.GetByIdAsync(Guid.NewGuid());

			Assert.IsNull(resultPost);
		}
		#endregion

		#region GetPostByCreatorAndTimeCreatedAsync
		[Test]
		public async Task GetPostByCreatorAndTimeCreatedAsync_ReturnsTheCorrectPost_IfItExists()
		{
			Post post = await this.AddEntity();

			Post resultPost = await this._postRepository.GetPostByCreatorAndTimeCreatedAsync(post.Creator.Id, post.TimeCreated);

			Assert.AreEqual(post.Id, resultPost.Id, "GetPostByCreatorAndTimeCreatedAsync does not return the corect post when it exists");
		}

		[Test]
		public async Task GetPostByCreatorAndTimeCreatedAsync_ReturnsNull_IfThePostDoesNotExist()
		{
			await this.AddEntity();

			Post resutPost = await this._postRepository.GetPostByCreatorAndTimeCreatedAsync(Guid.Empty, DateTime.Now);

			Assert.IsNull(resutPost, "GetPostByCreatorAndTimeCreatedAsync does not return null when the post does not exist");
		}
		#endregion

		#region DoesPostExist
		[Test]
		public async Task DoesPostExist_ReturnsTrue_WhenThePostExists()
		{
			Post post = await this.AddEntity();

			bool result = await this._postRepository.DoesPostExist(post.Id);

			Assert.IsTrue(result, "DoesPostExist does not return true whenm the Post exists");
		}

		[Test]
		public async Task DoesPostExist_ReturnsFalse_WhenThePostDoesNotExist()
		{
			bool result = await this._postRepository.DoesPostExist(Guid.Empty);

			Assert.IsFalse(result, "DoesPostExist does not return false whenm the Post does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task<Post> AddEntity()
		{
			User creator = new() { Id = Guid.NewGuid() };
			await this._context.Users.AddAsync(creator);
			Post post = new()
			{
				Message = POST_MESSAGE,
				Id = Guid.NewGuid(),
				Creator = creator,
				TimeCreated = DateTime.Now,
				Attachments = new List<PostAttachments> { new PostAttachments { FileUrl = "kur" }, new PostAttachments { FileUrl = "za" }, new PostAttachments { FileUrl = "tva" } },
				Comments = new List<Comment>()
			};

			await this._context.Posts.AddAsync(post);
			await this._context.SaveChangesAsync();

			return post;
		}
		#endregion
	}
}
