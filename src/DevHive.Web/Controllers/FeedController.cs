using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models;
using DevHive.Web.Models.Feed;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class FeedController
	{
		private readonly IFeedService _feedService;
		private readonly IMapper _mapper;

		public FeedController(IFeedService feedService, IMapper mapper)
		{
			this._feedService = feedService;
			this._mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetPosts(Guid userId, [FromBody] GetPageWebModel getPageWebModel)
		{
			GetPageServiceModel getPageServiceModel = this._mapper.Map<GetPageServiceModel>(getPageWebModel);
			getPageServiceModel.UserId = userId;

			ReadPageServiceModel readPageServiceModel = await this._feedService.GetPage(getPageServiceModel);
			ReadPageWebModel readPageWebModel = this._mapper.Map<ReadPageWebModel>(readPageServiceModel);

			return new OkObjectResult(readPageWebModel);
		}
	}
}
