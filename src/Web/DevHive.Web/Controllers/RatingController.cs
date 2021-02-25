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
	public class RatingController
	{
		private readonly IRatingService _rateService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public RatingController(IRatingService rateService, IUserService userService, IMapper mapper)
		{
			this._rateService = rateService;
			this._userService = userService;
			this._mapper = mapper;
		}

		[HttpPost]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> RatePost(Guid userId, [FromBody] CreateRatingWebModel createRatingWebModel, [FromHeader] string authorization)
		{
			CreateRatingServiceModel ratePostServiceModel = this._mapper.Map<CreateRatingServiceModel>(createRatingWebModel);
			ratePostServiceModel.UserId = userId;

			Guid id = await this._rateService.RatePost(ratePostServiceModel);

			if (Guid.Empty == id)
				return new BadRequestResult();

			return new OkObjectResult(id);
		}
	}
}
