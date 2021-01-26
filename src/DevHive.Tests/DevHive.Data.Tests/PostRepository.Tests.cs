using System;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class PostRepositoryTests
	{
		private const string POST_MESSAGE = "Post test message";

		protected DevHiveContext Context { get; set; }

		protected PostRepository PostRepository { get; set; }

		#region Setups
		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this.Context = new DevHiveContext(optionsBuilder.Options);

			PostRepository = new PostRepository(Context);
		}

		[TearDown]
		public void TearDown()
		{
			this.Context.Database.EnsureDeleted();
		}
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

			Post resultPost = await this.PostRepository.GetPostByCreatorAndTimeCreatedAsync(post.CreatorId, post.TimeCreated);

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
			Post post = new Post
			{
				Message = POST_MESSAGE,
				Id = Guid.NewGuid(),
				CreatorId = Guid.NewGuid(),
				TimeCreated = DateTime.Now
			};

			this.Context.Posts.Add(post);
			await this.Context.SaveChangesAsync();

			return post;
		}
		#endregion
	}
}
