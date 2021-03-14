using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Rating;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Rating;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class RatingControllerTests
	{
		private Mock<IRatingService> _ratingServiceMock;
		private Mock<IMapper> _mapperMock;
		private Mock<IJwtService> _jwtServiceMock;
		private RatingController _ratingController;

		[SetUp]
		public void SetUp()
		{
			this._ratingServiceMock = new Mock<IRatingService>();
			this._mapperMock = new Mock<IMapper>();
			this._jwtServiceMock = new Mock<IJwtService>();
			this._ratingController = new RatingController(this._ratingServiceMock.Object, this._mapperMock.Object, this._jwtServiceMock.Object);
		}

		#region Create
		[Test]
		public void CreateRating_ReturnsOkObjectResult_WhenRatingIsSuccessfullyCreated()
		{
			Guid postId = Guid.NewGuid();
			CreateRatingWebModel createRatingWebModel = new CreateRatingWebModel
			{
				PostId = postId,
				IsLike = true
			};
			CreateRatingServiceModel createRatingServiceModel = new CreateRatingServiceModel
			{
				PostId = postId,
				IsLike = true
			};
			Guid ratingId = Guid.NewGuid();

			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._mapperMock
				.Setup(p => p.Map<CreateRatingServiceModel>(It.IsAny<CreateRatingWebModel>()))
				.Returns(createRatingServiceModel);
			this._ratingServiceMock
				.Setup(p => p.RatePost(It.IsAny<CreateRatingServiceModel>()))
				.ReturnsAsync(ratingId);

			IActionResult result = this._ratingController.RatePost(Guid.Empty, createRatingWebModel, String.Empty).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			var splitted = (result as OkObjectResult).Value
				.ToString()
				.Split('{', '}', '=', ' ')
				.Where(x => !string.IsNullOrEmpty(x))
				.ToArray();

			Guid resultId = Guid.Parse(splitted[1]);

			Assert.AreEqual(ratingId, resultId);
		}

		[Test]
		public void CreateRating_ReturnsBadRequestResult_WhenRatingIsNotSuccessfullyCreated()
		{
			Guid postId = Guid.NewGuid();
			CreateRatingWebModel createRatingWebModel = new CreateRatingWebModel
			{
				PostId = postId,
				IsLike = true
			};
			CreateRatingServiceModel createRatingServiceModel = new CreateRatingServiceModel
			{
				PostId = postId,
				IsLike = true
			};
			Guid ratingId = Guid.NewGuid();

			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._mapperMock
				.Setup(p => p.Map<CreateRatingServiceModel>(It.IsAny<CreateRatingWebModel>()))
				.Returns(createRatingServiceModel);
			this._ratingServiceMock
				.Setup(p => p.RatePost(It.IsAny<CreateRatingServiceModel>()))
				.ReturnsAsync(Guid.Empty);

			IActionResult result = this._ratingController.RatePost(Guid.Empty, createRatingWebModel, String.Empty).Result;

			Assert.IsInstanceOf<BadRequestResult>(result);
		}
		#endregion

		#region Read
		[Test]
		public void GetRatingById_ReturnsTheRating_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			Guid postId = Guid.NewGuid();
			ReadRatingWebModel readRatingWebModel = new ReadRatingWebModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};

			this._mapperMock
				.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<ReadRatingWebModel>()))
				.Returns(readRatingServiceModel);
			this._ratingServiceMock
				.Setup(p => p.GetRatingById(It.IsAny<Guid>()))
				.ReturnsAsync(readRatingServiceModel);

			IActionResult result = this._ratingController.GetRatingById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public void GetRatingByUserAndPost_ReturnsTheRating_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			Guid postId = Guid.NewGuid();
			ReadRatingWebModel readRatingWebModel = new ReadRatingWebModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};

			this._mapperMock
				.Setup(p => p.Map<ReadRatingServiceModel>(It.IsAny<ReadRatingWebModel>()))
				.Returns(readRatingServiceModel);
			this._ratingServiceMock
				.Setup(p => p.GetRatingByPostAndUser(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(readRatingServiceModel);

			IActionResult result = this._ratingController.GetRatingByUserAndPost(userId, postId).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenRatingIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			Guid userId = Guid.NewGuid();
			Guid postId = Guid.NewGuid();
			UpdateRatingWebModel updateRatingWebModel = new UpdateRatingWebModel
			{
				IsLike = true
			};
			UpdateRatingServiceModel updateRatingServiceModel = new UpdateRatingServiceModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};
			ReadRatingWebModel readRatingWebModel = new ReadRatingWebModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};
			ReadRatingServiceModel readRatingServiceModel = new ReadRatingServiceModel
			{
				Id = id,
				UserId = userId,
				PostId = postId,
				IsLike = true
			};

			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._mapperMock
				.Setup(p => p.Map<UpdateRatingServiceModel>(It.IsAny<UpdateRatingWebModel>()))
				.Returns(updateRatingServiceModel);
			this._mapperMock
				.Setup(p => p.Map<ReadRatingWebModel>(It.IsAny<ReadRatingServiceModel>()))
				.Returns(readRatingWebModel);
			this._ratingServiceMock
				.Setup(p => p.UpdateRating(It.IsAny<UpdateRatingServiceModel>()))
				.ReturnsAsync(readRatingServiceModel);

			IActionResult result = this._ratingController.UpdateRating(userId, postId, updateRatingWebModel, String.Empty).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenLanguageIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdateRatingWebModel updateRatingWebModel = new UpdateRatingWebModel
			{
				IsLike = true
			};
			UpdateRatingServiceModel updateRatingServiceModel = new UpdateRatingServiceModel
			{
				Id = id,
				IsLike = true
			};

			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._mapperMock
				.Setup(p => p.Map<UpdateRatingServiceModel>(It.IsAny<UpdateRatingWebModel>()))
				.Returns(updateRatingServiceModel);
			this._ratingServiceMock
				.Setup(p => p.UpdateRating(It.IsAny<UpdateRatingServiceModel>()))
				.Returns(Task.FromResult<ReadRatingServiceModel>(null));

			IActionResult result = this._ratingController.UpdateRating(Guid.Empty, Guid.Empty, updateRatingWebModel, String.Empty).Result;

			Assert.IsInstanceOf<BadRequestResult>(result);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenRatingIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._ratingServiceMock
				.Setup(p => p.DeleteRating(It.IsAny<Guid>()))
				.ReturnsAsync(true);

			IActionResult result = this._ratingController.DeleteRating(Guid.Empty, Guid.Empty, String.Empty).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delete_ReturnsBadRequestObjectResult_WhenRatingIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Rating";
			Guid id = Guid.NewGuid();

			this._jwtServiceMock
				.Setup(p => p.ValidateToken(It.IsAny<Guid>(), It.IsAny<string>()))
				.Returns(true);
			this._ratingServiceMock
				.Setup(p => p.DeleteRating(It.IsAny<Guid>()))
				.ReturnsAsync(false);

			IActionResult result = this._ratingController.DeleteRating(Guid.Empty, Guid.Empty, String.Empty).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
