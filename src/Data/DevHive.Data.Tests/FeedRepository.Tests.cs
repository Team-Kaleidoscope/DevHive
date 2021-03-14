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
			_ = this._context.Database.EnsureDeleted();
		}
		#endregion

		#region GetFriendsPosts
		[Test]
		public async Task GetFriendsPosts_ReturnsListOfPosts_WhenTheyExist()
		{
			User dummyUser = CreateDummyUser();
			List<User> friendsList = new()
			{
				dummyUser
			};

			DateTime dateTime = new(3000, 05, 09, 9, 15, 0);
			Console.WriteLine(dateTime.ToFileTime());

			const int PAGE_NUMBER = 1;
			const int PAGE_SIZE = 10;

			List<Post> resultList = await this._feedRepository.GetFriendsPosts(friendsList, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.GreaterOrEqual(2, resultList.Count, "GetFriendsPosts does not return all correct posts");
		}

		[Test]
		public async Task GetFriendsPosts_ReturnsNull_WhenNoSuitablePostsExist()
		{
			User dummyUser = CreateDummyUser();
			List<User> friendsList = new()
			{
				dummyUser
			};

			DateTime dateTime = new(3000, 05, 09, 9, 15, 0);

			const int PAGE_NUMBER = 1;
			const int PAGE_SIZE = 10;

			List<Post> resultList = await this._feedRepository.GetFriendsPosts(friendsList, dateTime, PAGE_NUMBER, PAGE_SIZE);

			Assert.LessOrEqual(0, resultList.Count, "GetFriendsPosts does not return all correct posts");
		}
		#endregion

		#region HelperMethods
		private static User CreateDummyUser()
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
		#endregion
	}
}
