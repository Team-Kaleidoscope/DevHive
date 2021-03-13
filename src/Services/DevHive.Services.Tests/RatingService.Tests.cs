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
		private Mock<IPostRepository> PostRepositoryMock { get; set; }
		private Mock<IRatingRepository> RatingRepositoryMock { get; set; }
		private Mock<IUserRepository> UserRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private RatingService RatingService { get; set; }

		#region SetUps
		[SetUp]
		public void SetUp()
		{
			this.PostRepositoryMock = new Mock<IPostRepository>();
			this.RatingRepositoryMock = new Mock<IRatingRepository>();
			this.UserRepositoryMock = new Mock<IUserRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.RatingService = new RatingService(this.PostRepositoryMock.Object, this.RatingRepositoryMock.Object, this.UserRepositoryMock.Object, this.MapperMock.Object);
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

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(false));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(post));
			this.RatingRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Rating>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(rating));
			this.MapperMock.Setup(p => p.Map<Rating>(It.IsAny<CreateRatingServiceModel>())).Returns(rating);

			Guid result = await this.RatingService.RatePost(createRatingServiceModel);

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

			this.PostRepositoryMock.Setup(p => p.DoesPostExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(false));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.PostRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(post));
			this.RatingRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Rating>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Rating>(It.IsAny<CreateRatingServiceModel>())).Returns(rating);

			Guid result = await this.RatingService.RatePost(createRatingServiceModel);

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

			this.RatingRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(rating));
			this.MapperMock.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<Rating>())).Returns(readRatingServiceModel);

			ReadRatingServiceModel result = await this.RatingService.GetRatingById(id);

			Assert.AreEqual(isLike, result.IsLike);
		}

		[Test]
		public void GetRatingById_ThrowsException_WhenRatingDoesNotExist()
		{
			string exceptionMessage = "The rating does not exist";
			this.RatingRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Rating>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RatingService.GetRatingById(Guid.Empty));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}

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

			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(rating));
			this.MapperMock.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<Rating>())).Returns(readRatingServiceModel);

			ReadRatingServiceModel result = await this.RatingService.GetRatingByPostAndUser(user.Id, Guid.Empty);

			Assert.AreEqual(isLike, result.IsLike);
		}

		[Test]
		public void GetRatingByPostAndUser_ThrowsException_WhenRatingDoesNotExist()
		{
			string exceptionMessage = "The rating does not exist";
			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult<Rating>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RatingService.GetRatingById(Guid.Empty));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
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

			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(rating));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.RatingRepositoryMock.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Rating>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<Rating>())).Returns(readRatingServiceModel);

			ReadRatingServiceModel result = await this.RatingService.UpdateRating(updateRatingServiceModel);

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

			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(rating));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.RatingRepositoryMock.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Rating>())).Returns(Task.FromResult(false));

			ReadRatingServiceModel result = await this.RatingService.UpdateRating(updateRatingServiceModel);

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

			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult<Rating>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RatingService.UpdateRating(updateRatingServiceModel));

			Assert.AreEqual(ex.Message, exceptionMessage);
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

			this.RatingRepositoryMock.Setup(p => p.GetRatingByUserAndPostId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(rating));
			this.UserRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));
			this.RatingRepositoryMock.Setup(p => p.UserRatedPost(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RatingService.UpdateRating(updateRatingServiceModel));

			Assert.AreEqual(ex.Message, exceptionMessage);
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

			this.RatingRepositoryMock.Setup(p => p.DoesRatingExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Rating>(null));
			this.RatingRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Rating>())).Returns(Task.FromResult(true));

			bool result = await this.RatingService.DeleteRating(ratingId);

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

			this.RatingRepositoryMock.Setup(p => p.DoesRatingExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RatingRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Rating>(null));
			this.RatingRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Rating>())).Returns(Task.FromResult(false));

			bool result = await this.RatingService.DeleteRating(ratingId);

			Assert.IsFalse(result);
		}

		[Test]
		public void DeleteRating_ThrowsException_WhenRatingDoesNotExist()
		{
			string exceptionMessage = "Rating does not exist!";

			this.RatingRepositoryMock.Setup(p => p.DoesRatingExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RatingService.DeleteRating(Guid.Empty));

			Assert.AreEqual(ex.Message, exceptionMessage);
		}
		#endregion
	}
}
