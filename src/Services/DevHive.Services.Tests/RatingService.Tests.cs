using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Models.Rating;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
    [TestFixture]
	public class RatingServiceTests
	{
		private Mock<IPostRepository> _postRepositoryMock;
		private Mock<IRatingRepository> _ratingRepositoryMock;
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<IMapper> _mapperMock;
		private RatingService _ratingService;

		#region SetUps
		[SetUp]
		public void SetUp()
		{
			this._postRepositoryMock = new Mock<IPostRepository>();
			this._ratingRepositoryMock = new Mock<IRatingRepository>();
			this._userRepositoryMock = new Mock<IUserRepository>();
			this._mapperMock = new Mock<IMapper>();
			this._ratingService = new RatingService(this._postRepositoryMock.Object, this._ratingRepositoryMock.Object, this._userRepositoryMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region Create
		[Test]
		public async Task RatePost_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			bool isLike = true;
			Guid id = Guid.NewGuid();
			Guid postId = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			CreateRatingServiceModel createRatingServiceModel = new CreateRatingServiceModel
			{
				PostId = postId,
				UserId = userId,
				IsLike = isLike
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike
			};
			User user = new User
			{
				Id = userId
			};
			Post post = new Post
			{
				Id = postId
			};

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(false);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);
			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(post);
			this._ratingRepositoryMock
				.Setup(p => p.AddAsync(It.IsAny<Rating>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(rating);
			this._mapperMock
				.Setup(p => p.Map<Rating>(It.IsAny<CreateRatingServiceModel>()))
				.Returns(rating);

			Guid result = await this._ratingService.RatePost(createRatingServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task RatePost_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			bool isLike = true;
			Guid id = Guid.NewGuid();
			Guid postId = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			CreateRatingServiceModel createRatingServiceModel = new CreateRatingServiceModel
			{
				PostId = postId,
				UserId = userId,
				IsLike = isLike
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike
			};
			User user = new User
			{
				Id = userId
			};
			Post post = new Post
			{
				Id = postId
			};

			this._postRepositoryMock
				.Setup(p => p.DoesPostExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(false);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);
			this._postRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(post);
			this._ratingRepositoryMock
				.Setup(p => p.AddAsync(It.IsAny<Rating>()))
				.ReturnsAsync(false);
			this._mapperMock
				.Setup(p => p.Map<Rating>(It.IsAny<CreateRatingServiceModel>()))
				.Returns(rating);

			Guid result = await this._ratingService.RatePost(createRatingServiceModel);

			Assert.AreEqual(result, Guid.Empty);
		}
		#endregion

		#region Read
		[Test]
		public async Task GetRatingById_ReturnsTheRating_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			bool isLike = true;
			User user = new User
			{
				Id = Guid.NewGuid()
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike,
				User = user
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};

			this._ratingRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(rating);
			this._mapperMock
				.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<Rating>()))
				.Returns(readRatingServiceModel);

			ReadRatingServiceModel result = await this._ratingService.GetRatingById(id);

			Assert.AreEqual(isLike, result.IsLike);
		}

		// [Test]
		// public void GetRatingById_ThrowsException_WhenRatingDoesNotExist()
		// {
		// 	string exceptionMessage = "The rating does not exist";
		// 	this._ratingRepositoryMock
		// 		.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
		// 		.Returns(Task.FromResult<Rating>(null));
        //
		// 	Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._ratingService.GetRatingById(Guid.Empty));
        //
		// 	Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		// }

		[Test]
		public async Task GetRatingByPostAndUser_ReturnsTheRating_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			bool isLike = true;
			User user = new User
			{
				Id = Guid.NewGuid()
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike,
				User = user
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};

			this._ratingRepositoryMock
				.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(rating);
			this._mapperMock
				.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<Rating>()))
				.Returns(readRatingServiceModel);

			ReadRatingServiceModel result = await this._ratingService.GetRatingByPostAndUser(user.Id, Guid.Empty);

			Assert.AreEqual(isLike, result.IsLike);
		}

		// [Test]
		// public void GetRatingByPostAndUser_ThrowsException_WhenRatingDoesNotExist()
		// {
		// 	string exceptionMessage = "The rating does not exist";
		// 	this._ratingRepositoryMock
		// 		.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
		// 		.Returns(Task.FromResult<Rating>(null));
        //
		// 	Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._ratingService.GetRatingById(Guid.Empty));
        //
		// 	Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		// }
		#endregion

		#region Update
		[Test]
		public async Task UpdateRating_ReturnsObject_WhenRatingIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			bool isLike = true;
			UpdateRatingServiceModel updateRatingServiceModel = new UpdateRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};
			User user = new User
			{
				Id = Guid.NewGuid()
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike,
				User = user
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};

			this._ratingRepositoryMock
				.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(rating);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);
			this._ratingRepositoryMock
				.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Rating>()))
				.ReturnsAsync(true);
			this._mapperMock
				.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<Rating>()))
				.Returns(readRatingServiceModel);

			ReadRatingServiceModel result = await this._ratingService.UpdateRating(updateRatingServiceModel);

			Assert.AreEqual(result, readRatingServiceModel);
		}

		[Test]
		public async Task UpdateRating_ReturnsNull_WhenRatingIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			bool isLike = true;
			UpdateRatingServiceModel updateRatingServiceModel = new UpdateRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};
			User user = new User
			{
				Id = Guid.NewGuid()
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike,
				User = user
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};

			this._ratingRepositoryMock
				.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(rating);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);
			this._ratingRepositoryMock
				.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Rating>()))
				.ReturnsAsync(false);

			ReadRatingServiceModel result = await this._ratingService.UpdateRating(updateRatingServiceModel);

			Assert.IsNull(result);
		}

		[Test]
		public void UpdateRating_ThrowsException_WhenRatingDoesNotExists()
		{
			string exceptionMessage = "Rating does not exist!";
			UpdateRatingServiceModel updateRatingServiceModel = new UpdateRatingServiceModel
			{
				Id = Guid.Empty,
				IsLike = true
			};

			this._ratingRepositoryMock
				.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.Returns(Task.FromResult<Rating>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._ratingService.UpdateRating(updateRatingServiceModel));

			// Assert.AreEqual(ex.Message, exceptionMessage);
		}

		[Test]
		public void UpdateRating_ThrowsException_WhenUserHasNotRatedPost()
		{
			string exceptionMessage = "User has not rated the post!";
			Guid id = Guid.NewGuid();
			bool isLike = true;
			UpdateRatingServiceModel updateRatingServiceModel = new UpdateRatingServiceModel
			{
				Id = id,
				IsLike = isLike
			};
			User user = new User
			{
				Id = Guid.NewGuid()
			};
			Rating rating = new Rating
			{
				Id = id,
				IsLike = isLike,
				User = user
			};

			this._ratingRepositoryMock
				.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(rating);
			this._userRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(user);
			this._ratingRepositoryMock
				.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(false);

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._ratingService.UpdateRating(updateRatingServiceModel));

			// Assert.AreEqual(ex.Message, exceptionMessage);
		}
		#endregion

		#region Delete
		[Test]
		public async Task DeleteRating_ReturnsTrue_WhenRatingIsDeletedSuccessfully()
		{
			Guid ratingId = Guid.NewGuid();
			Rating rating = new Rating
			{
				Id = ratingId
			};

			this._ratingRepositoryMock
				.Setup(p => p.DoesRatingExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult<Rating>(null));
			this._ratingRepositoryMock
				.Setup(p => p.DeleteAsync(It.IsAny<Rating>()))
				.ReturnsAsync(true);

			bool result = await this._ratingService.DeleteRating(ratingId);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DeleteRating_ReturnsFalse_WhenRatingIsNotDeletedSuccessfully()
		{
			Guid ratingId = Guid.NewGuid();
			Rating rating = new Rating
			{
				Id = ratingId
			};

			this._ratingRepositoryMock
				.Setup(p => p.DoesRatingExist(It.IsAny<Guid>()))
				.ReturnsAsync(true);
			this._ratingRepositoryMock
				.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
				.Returns(Task.FromResult<Rating>(null));
			this._ratingRepositoryMock
				.Setup(p => p.DeleteAsync(It.IsAny<Rating>()))
				.ReturnsAsync(false);

			bool result = await this._ratingService.DeleteRating(ratingId);

			Assert.IsFalse(result);
		}

		[Test]
		public void DeleteRating_ThrowsException_WhenRatingDoesNotExist()
		{
			string exceptionMessage = "Rating does not exist!";

			this._ratingRepositoryMock
				.Setup(p => p.DoesRatingExist(It.IsAny<Guid>()))
				.ReturnsAsync(false);

			Exception ex = Assert.ThrowsAsync<ArgumentNullException>(() => this._ratingService.DeleteRating(Guid.Empty));

			// Assert.AreEqual(ex.Message, exceptionMessage);
		}
		#endregion
	}
}
