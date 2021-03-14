using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Comment;
using DevHive.Web.Models.Feed;
using DevHive.Web.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class FeedControllerTests
	{
		private Mock<IFeedService> _feedServiceMock;
		private Mock<IMapper> _mapperMock;
		private FeedController _feedController;

		#region SetUp
		[SetUp]
		public void SetUp()
		{
			this._feedServiceMock = new Mock<IFeedService>();
			this._mapperMock = new Mock<IMapper>();
			this._feedController = new FeedController(this._feedServiceMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region GetPosts
		[Test]
		public async Task GetPosts_ReturnsOkObjectResultWithCorrectReadPageWebModel_WhenPostsExist()
		{
			GetPageWebModel getPageWebModel = new GetPageWebModel { };
			GetPageServiceModel getPageServiceModel = new GetPageServiceModel { };
			ReadPageServiceModel readPageServiceModel = new ReadPageServiceModel { };
			ReadPageWebModel readPageWebModel = new ReadPageWebModel
			{
				Posts = new List<ReadPostWebModel>
				{
					new ReadPostWebModel(),
					new ReadPostWebModel(),
					new ReadPostWebModel()
				}
			};

			this._feedServiceMock.Setup(p => p.GetPage(It.IsAny<GetPageServiceModel>())).Returns(Task.FromResult(readPageServiceModel));
			this._mapperMock.Setup(p => p.Map<GetPageServiceModel>(It.IsAny<GetPageWebModel>())).Returns(getPageServiceModel);
			this._mapperMock.Setup(p => p.Map<ReadPageWebModel>(It.IsAny<ReadPageServiceModel>())).Returns(readPageWebModel);

			IActionResult result = await this._feedController.GetPosts(Guid.Empty, getPageWebModel);

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadPageWebModel resultModel = okObjectResult.Value as Models.Comment.ReadPageWebModel;

			Assert.AreEqual(3, resultModel.Posts.Count);
		}
		#endregion

		#region GetUserPosts
		[Test]
		public async Task GetUserPosts_GetsPostsOfUser_WhenTheyExist()
		{
			GetPageWebModel getPageWebModel = new GetPageWebModel { };
			GetPageServiceModel getPageServiceModel = new GetPageServiceModel { };
			ReadPageServiceModel readPageServiceModel = new ReadPageServiceModel { };
			ReadPageWebModel readPageWebModel = new ReadPageWebModel
			{
				Posts = new List<ReadPostWebModel>
				{
					new ReadPostWebModel(),
					new ReadPostWebModel(),
					new ReadPostWebModel()
				}
			};

			this._feedServiceMock.Setup(p => p.GetUserPage(It.IsAny<GetPageServiceModel>())).Returns(Task.FromResult(readPageServiceModel));
			this._mapperMock.Setup(p => p.Map<GetPageServiceModel>(It.IsAny<GetPageWebModel>())).Returns(getPageServiceModel);
			this._mapperMock.Setup(p => p.Map<ReadPageWebModel>(It.IsAny<ReadPageServiceModel>())).Returns(readPageWebModel);

			IActionResult result = await this._feedController.GetUserPosts(null, getPageWebModel);

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadPageWebModel resultModel = okObjectResult.Value as Models.Comment.ReadPageWebModel;

			Assert.AreEqual(3, resultModel.Posts.Count);
		}
		#endregion
	}
}
