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
		protected DevHiveContext Context { get; set; }

		protected FeedRepository FeedRepository { get; set; }

		#region Setups
		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this.Context = new DevHiveContext(optionsBuilder.Options);

			FeedRepository = new FeedRepository(Context);
		}

		[TearDown]
		public void TearDown()
		{
			this.Context.Database.EnsureDeleted();
		}
		#endregion

		#region GetFriendsPosts
		[Test]
		public async Task GetFriendsPosts_ReturnsListOfPosts_WhenTheyExist()
		{
			User dummyUser = this.CreateDummyUser();
			List<User> friendsList = new List<User>();
			friendsList.Add(dummyUser);

			DateTime dateTime = new DateTime(3000, 05, 09, 9, 15, 0);

			Post dummyPost = this.CreateDummyPost(dummyUser.Id);
			Post anotherDummnyPost = this.CreateDummyPost(dummyUser.Id);

			const int PAGE_NUMBER = 1;
			const int PAGE_SIZE = 10;

			List<Post> resultList = await this.FeedRepository.GetFriendsPosts(friendsList, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.GreaterOrEqual(2, resultList.Count, "GetFriendsPosts does not return all correct posts");
		}

		[Test]
		public async Task GetFriendsPosts_ReturnsNull_WhenNoSuitablePostsExist()
		{
			User dummyUser = this.CreateDummyUser();
			List<User> friendsList = new List<User>();
			friendsList.Add(dummyUser);

			DateTime dateTime = new DateTime(3000, 05, 09, 9, 15, 0);

			const int PAGE_NUMBER = 1;
			const int PAGE_SIZE = 10;

			List<Post> resultList = await this.FeedRepository.GetFriendsPosts(friendsList, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.LessOrEqual(0, resultList.Count, "GetFriendsPosts does not return all correct posts");
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

		private Post CreateDummyPost(Guid posterId)
		{
			const string POST_MESSAGE = "random message";
			Guid id = Guid.NewGuid();

			Post post = new Post
			{
				Id = id,
				Message = POST_MESSAGE,
				CreatorId = posterId
			};

			this.Context.Posts.Add(post);
			this.Context.SaveChanges();

			return post;
		}
		#endregion
	}
}
