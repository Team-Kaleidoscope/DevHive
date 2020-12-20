using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Identity.User;
using DevHive.Services.Options;
using DevHive.Services.Services;
using DevHive.Web.Models.Identity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevHive.Common.Models.Identity;
using DevHive.Common.Models.Data;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	[Authorize(Roles = "User")]
	public class UserController: ControllerBase
	{
		private readonly UserService _userService;
		private readonly IMapper _userMapper;

		public UserController(UserService userService, IMapper mapper)
		{
			this._userService = userService;
			this._userMapper = mapper;
		}

		[HttpPost]
		[Route("Login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginWebModel loginModel)
		{
			LoginServiceModel loginServiceModel = this._userMapper.Map<LoginServiceModel>(loginModel);

			TokenModel TokenModel = await this._userService.LoginUser(loginServiceModel);
			TokenWebModel tokenWebModel = this._userMapper.Map<TokenWebModel>(TokenModel);

			return new OkObjectResult(tokenWebModel);
		}

		//Create
		[HttpPost]
		[Route("Register")]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] RegisterWebModel registerModel)
		{
			RegisterServiceModel registerServiceModel = this._userMapper.Map<RegisterServiceModel>(registerModel);

			TokenModel TokenModel = await this._userService.RegisterUser(registerServiceModel);
			TokenWebModel tokenWebModel = this._userMapper.Map<TokenWebModel>(TokenModel);

			return new CreatedResult("Register", tokenWebModel);
		}

		[HttpPost]
		[Route("AddAFriend")]
		public async Task<IActionResult> AddAFriend(Guid userId, [FromBody] IdModel friendIdModel)
		{
			return await this._userService.AddFriend(userId, friendIdModel.Id) ?
				new OkResult() :
				new BadRequestResult();
		}

		//Read
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			UserServiceModel userServiceModel = await this._userService.GetUserById(id);
			UserWebModel userWebModel = this._userMapper.Map<UserWebModel>(userServiceModel);

			return new OkObjectResult(userWebModel);
		}

		[HttpGet]
		[Route("GetAFriend")]
		public async Task<IActionResult> GetAFriend(Guid friendId)
		{
			UserServiceModel friendServiceModel = await this._userService.GetFriendById(friendId);
			UserWebModel friend = this._userMapper.Map<UserWebModel>(friendServiceModel);

			return new OkObjectResult(friend);
		}

		//Update
		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserWebModel updateModel, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			UpdateUserServiceModel updateUserServiceModel = this._userMapper.Map<UpdateUserServiceModel>(updateModel);
			updateUserServiceModel.Id = id;

			UserServiceModel userServiceModel = await this._userService.UpdateUser(updateUserServiceModel);
			UserWebModel userWebModel = this._userMapper.Map<UserWebModel>(userServiceModel);

			return new AcceptedResult("UpdateUser", userWebModel);

		}

		//Delete
		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			await this._userService.DeleteUser(id);
			return new OkResult();
		}

		[HttpDelete]
		[Route("RemoveAFriend")]
		public async Task<IActionResult> RemoveAFriend(Guid userId, Guid friendId)
		{
			await this._userService.RemoveFriend(userId, friendId);
			return new OkResult();
		}
	}
}
