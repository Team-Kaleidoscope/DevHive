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
		private Mock<IFeedService> FeedServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private FeedController FeedController { get; set; }

		#region SetUp
		[SetUp]
		public void SetUp()
		{
			this.FeedServiceMock = new();
			this.MapperMock = new();
			this.FeedController = new(this.FeedServiceMock.Object, this.MapperMock.Object);
		}
		#endregion

		#region GetPosts
		[Test]
		public async Task GetPosts_ReturnsOkObjectResultWithCorrectReadPageWebModel_WhenPostsExist()
		{
			GetPageWebModel getPageWebModel = new();
			GetPageServiceModel getPageServiceModel = new();
			ReadPageServiceModel readPageServiceModel = new();
			ReadPageWebModel readPageWebModel = new()
			{
				Posts = new()
				{
					new ReadPostWebModel(),
					new ReadPostWebModel(),
					new ReadPostWebModel()
				}
			};

			this.FeedServiceMock.Setup(p => p.GetPage(It.IsAny<GetPageServiceModel>())).Returns(Task.FromResult(readPageServiceModel));
			this.MapperMock.Setup(p => p.Map<GetPageServiceModel>(It.IsAny<GetPageWebModel>())).Returns(getPageServiceModel);
			this.MapperMock.Setup(p => p.Map<ReadPageWebModel>(It.IsAny<ReadPageServiceModel>())).Returns(readPageWebModel);

			IActionResult result = await this.FeedController.GetPosts(Guid.Empty, getPageWebModel);

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
			GetPageWebModel getPageWebModel = new();
			GetPageServiceModel getPageServiceModel = new();
			ReadPageServiceModel readPageServiceModel = new();
			ReadPageWebModel readPageWebModel = new()
			{
				Posts = new()
				{
					new ReadPostWebModel(),
					new ReadPostWebModel(),
					new ReadPostWebModel()
				}
			};

			this.FeedServiceMock.Setup(p => p.GetUserPage(It.IsAny<GetPageServiceModel>())).Returns(Task.FromResult(readPageServiceModel));
			this.MapperMock.Setup(p => p.Map<GetPageServiceModel>(It.IsAny<GetPageWebModel>())).Returns(getPageServiceModel);
			this.MapperMock.Setup(p => p.Map<ReadPageWebModel>(It.IsAny<ReadPageServiceModel>())).Returns(readPageWebModel);

			IActionResult result = await this.FeedController.GetUserPosts(null, getPageWebModel);

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadPageWebModel resultModel = okObjectResult.Value as Models.Comment.ReadPageWebModel;

			Assert.AreEqual(3, resultModel.Posts.Count);
		}
		#endregion
	}
}
