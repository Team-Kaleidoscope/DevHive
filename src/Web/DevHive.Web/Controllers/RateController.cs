using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Post.Rating;
using DevHive.Web.Models.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RateController
	{
		private readonly IRatingService _rateService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public RateController(IRatingService rateService, IUserService userService, IMapper mapper)
		{
			this._rateService = rateService;
			this._userService = userService;
			this._mapper = mapper;
		}

		[HttpPost]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> RatePost(Guid userId, [FromBody] RatePostWebModel ratePostWebModel, [FromHeader] string authorization)
		{
			CreateRatingServiceModel ratePostServiceModel = this._mapper.Map<CreateRatingServiceModel>(ratePostWebModel);
			ratePostServiceModel.UserId = userId;

			ReadRatingServiceModel readPostRatingServiceModel = await this._rateService.RatePost(ratePostServiceModel);
			ReadPostRatingWebModel readPostRatingWebModel = this._mapper.Map<ReadPostRatingWebModel>(readPostRatingServiceModel);

			return new OkObjectResult(readPostRatingWebModel);
		}
	}
}
