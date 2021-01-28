using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models;
using DevHive.Services.Models.Post.Post;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class FeedServiceTests
	{
		private Mock<IFeedRepository> FeedRepositoryMock { get; set; }
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private FeedService FeedService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.FeedRepositoryMock = new Mock<IFeedRepository>();
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.FeedService = new FeedService(this.FeedRepositoryMock.Object, this.UserRepositoryMock.Object, this.MapperMock.Object);
		}
		#endregion

		#region GetPage
		[Test]
		public async Task GetPage_ReturnsReadPageServiceModel_WhenSuitablePostsExist()
		{
			GetPageServiceModel getPageServiceModel = new GetPageServiceModel
			{
				UserId = Guid.NewGuid()
			};

			User dummyUser = CreateDummyUser();
			User anotherDummyUser = CreateAnotherDummyUser();
			HashSet<User> friends = new HashSet<User>();
			friends.Add(anotherDummyUser);
			dummyUser.Friends = friends;

			List<Post> posts = new List<Post>
			{
				new Post{ Message = "Message"}
			};

			ReadPostServiceModel readPostServiceModel = new ReadPostServiceModel
			{
				PostId = Guid.NewGuid(),
				Message = "Message"
			};
			List<ReadPostServiceModel> readPostServiceModels = new List<ReadPostServiceModel>();
			readPostServiceModels.Add(readPostServiceModel);
			ReadPageServiceModel readPageServiceModel = new ReadPageServiceModel
			{
				Posts = readPostServiceModels
			};

			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(dummyUser));
			this.FeedRepositoryMock.Setup(p => p.GetFriendsPosts(It.IsAny<List<User>>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(posts));
			this.MapperMock.Setup(p => p.Map<ReadPostServiceModel>(It.IsAny<Post>())).Returns(readPostServiceModel);

			ReadPageServiceModel result = await this.FeedService.GetPage(getPageServiceModel);

			Assert.GreaterOrEqual(1, result.Posts.Count, "GetPage does not correctly return the posts");
		}

		[Test]
		public void GetPage_ThrowsException_WhenNoSuitablePostsExist()
		{
			const string EXCEPTION_MESSAGE = "No friends of user have posted anything yet!";
			GetPageServiceModel getPageServiceModel = new GetPageServiceModel
			{
				UserId = Guid.NewGuid()
			};

			User dummyUser = CreateDummyUser();
			User anotherDummyUser = CreateAnotherDummyUser();
			HashSet<User> friends = new HashSet<User>();
			friends.Add(anotherDummyUser);
			dummyUser.Friends = friends;

			ReadPostServiceModel readPostServiceModel = new ReadPostServiceModel
			{
				PostId = Guid.NewGuid(),
				Message = "Message"
			};
			List<ReadPostServiceModel> readPostServiceModels = new List<ReadPostServiceModel>();
			readPostServiceModels.Add(readPostServiceModel);
			ReadPageServiceModel readPageServiceModel = new ReadPageServiceModel
			{
				Posts = readPostServiceModels
			};

			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(dummyUser));
			this.FeedRepositoryMock.Setup(p => p.GetFriendsPosts(It.IsAny<List<User>>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new List<Post>()));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.FeedService.GetPage(getPageServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Wrong exception message");
		}

		[Test]
		public void GetPage_ThrowsException_WhenUserHasNoFriendsToGetPostsFrom()
		{
			const string EXCEPTION_MESSAGE = "User has no friends to get feed from!";
			GetPageServiceModel getPageServiceModel = new GetPageServiceModel
			{
				UserId = Guid.NewGuid()
			};

			User dummyUser = CreateDummyUser();

			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(dummyUser));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.FeedService.GetPage(getPageServiceModel));

			Assert.AreEqual(EXCEPTION_MESSAGE, ex.Message, "Wrong exception message");
		}
		#endregion

		#region HelperMethods
		private User CreateDummyUser()
		{
			return new()
			{
				Id = Guid.NewGuid(),
				UserName = "dummyUser",
				FirstName = "Spas",
				LastName = "Spasov",
				Email = "abv@abv.bg",
			};
		}

		private User CreateAnotherDummyUser()
		{
			return new()
			{
				Id = Guid.NewGuid(),
				UserName = "anotherDummyUser",
				FirstName = "Alex",
				LastName = "Spiridonov",
				Email = "a_spiridonov@abv.bg",
			};
		}
		#endregion
	}
}
