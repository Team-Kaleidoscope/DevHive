using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class FeedRepositoryTests
	{
		private const int PAGE_NUMBER = 1;
		private const int PAGE_SIZE = 10;

		private DevHiveContext _context;
		private FeedRepository _feedRepository;

		#region Setups
		[SetUp]
		public void Setup()
		{
			DbContextOptionsBuilder<DevHiveContext> optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this._context = new DevHiveContext(optionsBuilder.Options);

			this._feedRepository = new FeedRepository(this._context);
		}

		[TearDown]
		public void TearDown()
		{
			this._context.Database.EnsureDeleted();
		}
		#endregion

		#region GetFriendsPosts
		[Test]
		public async Task GetFriendsPosts_ReturnsListOfPosts_WhenTheyExist()
		{
			User dummyUser = this.CreateDummyUser();
			dummyUser.Posts = await this.CreateDummyPosts(dummyUser);
			List<User> friendsList = new()
			{
				dummyUser
			};

			DateTime dateTime = DateTime.Now;

			List<Post> resultList = await this._feedRepository.GetFriendsPosts(friendsList, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.GreaterOrEqual(resultList.Count, dummyUser.Posts.Count, "GetFriendsPosts does not return the posts corrtrectly");
		}

		[Test]
		public async Task GetFriendsPosts_ReturnsEmptyList_WhenNoSuitablePostsExist()
		{
			User dummyUser = this.CreateDummyUser();
			List<User> friendsList = new()
			{
				dummyUser
			};

			DateTime dateTime = DateTime.Now;

			List<Post> resultList = await this._feedRepository.GetFriendsPosts(friendsList, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.LessOrEqual(resultList.Count, 0, "GetFriendsPosts does not return all correct posts");
		}
		#endregion

		#region GetUsersPosts
		[Test]
		public async Task GetUsersPosts_ReturnsAllPostsOfTheUser_IfAnyExist()
		{
			User dummyUser = this.CreateDummyUser();
			HashSet<Post> posts = await this.CreateDummyPosts(dummyUser);

			DateTime dateTime = DateTime.Now;

			List<Post> resultList = await this._feedRepository.GetUsersPosts(dummyUser, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.GreaterOrEqual(resultList.Count, posts.Count, "GetUsersPosts does not return the posts corrtrectly");
		}

		[Test]
		public async Task GetUsersPosts_ReturnsEmptyList_WhenNoSuitablePostsExist()
		{
			User dummyUser = this.CreateDummyUser();

			DateTime dateTime = DateTime.Now;

			List<Post> resultList = await this._feedRepository.GetUsersPosts(dummyUser, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.LessOrEqual(resultList.Count, 0, "GetUsersPosts does not return empty list when no suitable posts exist");
		}
		#endregion

		#region HelperMethods
		private User CreateDummyUser()
		{
			HashSet<Role> roles = new()
			{
				new Role()
				{
					Id = Guid.NewGuid(),
					Name = Role.DefaultRole
				},
			};

			return new()
			{
				Id = Guid.NewGuid(),
				UserName = "pioneer10",
				FirstName = "Spas",
				LastName = "Spasov",
				Email = "abv@abv.bg",
				Roles = roles
			};
		}

		private async Task<HashSet<Post>> CreateDummyPosts(User user)
		{
			HashSet<Post> posts = new HashSet<Post>
			{
				new Post { Creator = user, TimeCreated = DateTime.Now },
				new Post{ Creator = user, TimeCreated = DateTime.Now },
				new Post{ Creator = user, TimeCreated = DateTime.Now }
			};

			await this._context.Posts.AddRangeAsync(posts);
			await this._context.SaveChangesAsync();

			return posts;
		}
		#endregion
	}
}
