using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using DevHive.Data.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DevHive.Data.Tests
{
    [TestFixture]
	public class RatingRepositoryTests
	{
		private DevHiveContext Context { get; set; }
		private RatingRepository RatingRepository { get; set; }

		#region Setups
		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this.Context = new DevHiveContext(optionsBuilder.Options);
			this.RatingRepository = new RatingRepository(this.Context, null);
		}

		[TearDown]
		public void TearDown()
		{
			this.Context.Database.EnsureDeleted();
		}
		#endregion

		#region GetById
		[Test]
		public async Task GetByIdAsync_ReturnsTheCorrectRating_IfItExists()
		{
			Guid ratingId = Guid.NewGuid();
			await AddDummyRating(ratingId);

			Rating ratingResult = await this.RatingRepository.GetByIdAsync(ratingId);

			Assert.AreEqual(ratingResult.Id, ratingId);
		}

		[Test]
		public async Task GetByIdAsync_ReturnsNull_IfRatingDoesNotExist()
		{
			Rating ratingResult = await this.RatingRepository.GetByIdAsync(Guid.NewGuid());

			Assert.IsNull(ratingResult);
		}
		#endregion

		#region GetByPostId
		[Test]
		public async Task GetRatingsByPostId_ReturnsFilledListOfRatings_WhenTheyExist()
		{
			Guid postId = Guid.NewGuid();
			await AddDummyPost(postId);
			await AddDummyRating(Guid.NewGuid(), postId);
			await AddDummyRating(Guid.NewGuid(), postId);

			List<Rating> result = await this.RatingRepository.GetRatingsByPostId(postId);

			Assert.IsNotEmpty(result);
		}

		[Test]
		public async Task GetRatingsByPostId_ReturnsEmptyList_WhenThereAreNoRatings()
		{
			List<Rating> result = await this.RatingRepository.GetRatingsByPostId(Guid.NewGuid());

			Assert.IsEmpty(result);
		}
		#endregion

		#region GetByUserAndPostId
		[Test]
		public async Task GetRatingByUserAndPostId_ReturnsRating_WhenItExists()
		{
			Guid ratingId = Guid.NewGuid();
			Guid postId = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			await AddDummyPost(postId);
			await AddDummyUser(userId);
			await AddDummyRating(ratingId, postId, userId);

			Rating result = await this.RatingRepository.GetRatingByUserAndPostId(userId, postId);

			Assert.AreEqual(result.Id, ratingId);
		}

		[Test]
		public async Task GetRatingByUserAndPostId_ReturnsNull_WhenRatingDoesNotExist()
		{
			Rating result = await this.RatingRepository.GetRatingByUserAndPostId(Guid.NewGuid(), Guid.NewGuid());

			Assert.IsNull(result);
		}
		#endregion

		#region UserRatedPost
		[Test]
		public async Task UserRatedPost_ReturnsTrue_WhenUserHasRatedPost()
		{
			Guid postId = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			await AddDummyPost(postId);
			await AddDummyUser(userId);
			await AddDummyRating(Guid.NewGuid(), postId, userId);

			bool result = await this.RatingRepository.UserRatedPost(userId, postId);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task UserRatedPost_ReturnsFalse_WhenUserHasNotRatedPost()
		{
			bool result = await this.RatingRepository.UserRatedPost(Guid.NewGuid(), Guid.NewGuid());

			Assert.IsFalse(result);
		}
		#endregion

		#region DoesRatingExist
		[Test]
		public async Task DoesRatingExist_ReturnsTrue_WhenItExists()
		{
			Guid ratingId = Guid.NewGuid();
			await AddDummyRating(ratingId);

			bool result = await this.RatingRepository.DoesRatingExist(ratingId);

			Assert.IsTrue(result);
		}

		[Test]
		public async Task DoesRatingExist_ReturnsFalse_WhenRatingDoesNotExist()
		{
			bool result = await this.RatingRepository.DoesRatingExist(Guid.NewGuid());

			Assert.IsFalse(result);
		}
		#endregion

		#region HelperMethods
		private async Task AddDummyRating(Guid ratingId, Guid postId = default(Guid), Guid userId = default(Guid))
		{
			Rating rating = new Rating
			{
				Id = ratingId,
				Post = this.Context.Posts.FirstOrDefault(x => x.Id == postId),
				User = this.Context.Users.FirstOrDefault(x => x.Id == userId)
			};

			await this.Context.Rating.AddAsync(rating);
			await this.Context.SaveChangesAsync();
		}

		private async Task AddDummyPost(Guid postId)
		{
			Post post = new Post()
			{
				Id = postId,
				Message = "Never gonna give you up"
			};

			await this.Context.Posts.AddAsync(post);
			await this.Context.SaveChangesAsync();
		}

		private async Task AddDummyUser(Guid userId)
		{
			User user = new User()
			{
				Id = userId
			};

			await this.Context.Users.AddAsync(user);
			await this.Context.SaveChangesAsync();
		}
		#endregion
	}
}
