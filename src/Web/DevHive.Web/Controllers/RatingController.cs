using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Rating;
using DevHive.Web.Models.Rating;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Authorize(Roles = "Admin,User")]
	[Route("api/[controller]")]
	public class RatingController
	{
		private readonly IRatingService _rateService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly IJwtService _jwtService;

		public RatingController(IRatingService rateService, IUserService userService, IMapper mapper, IJwtService jwtService)
		{
			this._rateService = rateService;
			this._userService = userService;
			this._mapper = mapper;
			this._jwtService = jwtService;
		}

		[HttpPost]
		public async Task<IActionResult> RatePost(Guid userId, [FromBody] CreateRatingWebModel createRatingWebModel, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			CreateRatingServiceModel ratePostServiceModel = this._mapper.Map<CreateRatingServiceModel>(createRatingWebModel);
			ratePostServiceModel.UserId = userId;

			Guid id = await this._rateService.RatePost(ratePostServiceModel);

			if (Guid.Empty == id)
				return new BadRequestResult();

			return new OkObjectResult(new { Id = id });
		}

		[HttpGet]
		public async Task<IActionResult> GetRatingById(Guid id)
		{
			ReadRatingServiceModel readRatingServiceModel = await this._rateService.GetRatingById(id);
			ReadRatingWebModel readPostRatingWebModel = this._mapper.Map<ReadRatingWebModel>(readRatingServiceModel);

			return new OkObjectResult(readPostRatingWebModel);
		}

		[HttpGet]
		[Route("GetByUserAndPost")]
		public async Task<IActionResult> GetRatingByUserAndPost(Guid userId, Guid postId)
		{
			ReadRatingServiceModel readRatingServiceModel = await this._rateService.GetRatingByPostAndUser(userId, postId);
			ReadRatingWebModel readPostRatingWebModel = this._mapper.Map<ReadRatingWebModel>(readRatingServiceModel);

			return new OkObjectResult(readPostRatingWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateRating(Guid userId, Guid postId, [FromBody] UpdateRatingWebModel updateRatingWebModel, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			UpdateRatingServiceModel updateRatingServiceModel =
				this._mapper.Map<UpdateRatingServiceModel>(updateRatingWebModel);
			updateRatingServiceModel.UserId = userId;
			updateRatingServiceModel.PostId = postId;

			ReadRatingServiceModel readRatingServiceModel = await this._rateService.UpdateRating(updateRatingServiceModel);

			if (readRatingServiceModel == null)
				return new BadRequestResult();
			else
			{
				ReadRatingWebModel readRatingWebModel = this._mapper.Map<ReadRatingWebModel>(readRatingServiceModel);
				return new OkObjectResult(readRatingWebModel);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteTating(Guid userId, Guid ratingId, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			return await this._rateService.DeleteRating(ratingId) ?
				new OkResult() :
				new BadRequestObjectResult("Could not delete Rating");
		}
	}
}
