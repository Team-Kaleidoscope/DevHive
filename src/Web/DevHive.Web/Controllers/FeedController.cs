using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models;
using DevHive.Web.Models.Comment;
using DevHive.Web.Models.Feed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the feed layer
	/// </summary>
	[ApiController]
	[Route("/api/[controller]")]
	[Authorize(Roles = "User,Admin")]
	public class FeedController
	{
		private readonly IFeedService _feedService;
		private readonly IMapper _mapper;

		public FeedController(IFeedService feedService, IMapper mapper)
		{
			this._feedService = feedService;
			this._mapper = mapper;
		}

		/// <summary>
		/// Query posts for user's feed
		/// </summary>
		/// <param name="userId">The user's Id, whose feed is begin queried</param>
		/// <param name="getPageWebModel">Page parametars</param>
		/// <returns>A page of the feed</returns>
		[HttpPost]
		[Route("GetPosts")]
		public async Task<IActionResult> GetPosts(Guid userId, [FromBody] GetPageWebModel getPageWebModel)
		{
			GetPageServiceModel getPageServiceModel = this._mapper.Map<GetPageServiceModel>(getPageWebModel);
			getPageServiceModel.UserId = userId;

			ReadPageServiceModel readPageServiceModel = await this._feedService.GetPage(getPageServiceModel);
			ReadPageWebModel readPageWebModel = this._mapper.Map<ReadPageWebModel>(readPageServiceModel);

			return new OkObjectResult(readPageWebModel);
		}

		/// <summary>
		/// Query a user profile's posts
		/// </summary>
		/// <param name="username">The user's username, whose posts are being queried</param>
		/// <param name="getPageWebModel">Page parametars</param>
		/// <returns>A page of the user's posts</returns>
		[HttpPost]
		[Route("GetUserPosts")]
		[AllowAnonymous]
		public async Task<IActionResult> GetUserPosts(string username, [FromBody] GetPageWebModel getPageWebModel)
		{
			GetPageServiceModel getPageServiceModel = this._mapper.Map<GetPageServiceModel>(getPageWebModel);
			getPageServiceModel.Username = username;

			ReadPageServiceModel readPageServiceModel = await this._feedService.GetUserPage(getPageServiceModel);
			ReadPageWebModel readPageWebModel = this._mapper.Map<ReadPageWebModel>(readPageServiceModel);

			return new OkObjectResult(readPageWebModel);
		}
	}
}
