using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Identity.User;
using DevHive.Services.Options;
using DevHive.Services.Services;
using DevHive.Web.Models.Identity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api")]
	[Authorize(Roles = "User")]
	public class FriendsController
	{
		private readonly FriendsService _friendsService;
		private readonly IMapper _friendsMapper;

		public FriendsController(DevHiveContext context, IMapper mapper, JWTOptions jwtOptions)
		{
			this._friendsService = new FriendsService(context, mapper, jwtOptions);
			this._friendsMapper = mapper;
		}

		//Create
		[HttpPost]
		[Route("AddAFriend")]
		public async Task<IActionResult> AddAFriend(Guid userId, [FromBody] IdModel friendIdModel)
		{
			return await this._friendsService.AddFriend(userId, friendIdModel.Id) ?
				new OkResult() :
				new BadRequestResult();
		}

		//Read
		[HttpGet]
		[Route("GetAFriend")]
		public async Task<IActionResult> GetAFriend(Guid friendId)
		{
			UserServiceModel friendServiceModel = await this._friendsService.GetFriendById(friendId);
			UserWebModel friend = this._friendsMapper.Map<UserWebModel>(friendServiceModel);

			return new OkObjectResult(friend);
		}
	
		//Delete
		[HttpDelete]
		[Route("RemoveAFriend")]
		public async Task<IActionResult> RemoveAFriend(Guid userId, Guid friendId)
		{
			await this._friendsService.RemoveFriend(userId, friendId);
			return new OkResult();
		}
	}
}