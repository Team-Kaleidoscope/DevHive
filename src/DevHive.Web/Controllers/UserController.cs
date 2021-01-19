using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Models.Identity.User;
using DevHive.Web.Models.Identity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevHive.Common.Models.Identity;
using DevHive.Common.Models.Misc;
using DevHive.Web.Models.Language;
using DevHive.Services.Models.Language;
using DevHive.Web.Models.Technology;
using DevHive.Services.Models.Technology;
using DevHive.Services.Interfaces;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	[Authorize(Roles = "User")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _userMapper;

		public UserController(IUserService userService, IMapper mapper)
		{
			this._userService = userService;
			this._userMapper = mapper;
		}

		#region Authentication
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
		#endregion

		#region Read

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
		[Route("GetUser")]
		[AllowAnonymous]
		public async Task<IActionResult> GetUser(string username)
		{
			UserServiceModel friendServiceModel = await this._userService.GetUserByUsername(username);
			UserWebModel friend = this._userMapper.Map<UserWebModel>(friendServiceModel);

			return new OkObjectResult(friend);
		}
		#endregion

		#region Update
		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserWebModel updateModel, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			// if (!ModelState.IsValid)
			// 	return BadRequest("Not a valid model!");

			UpdateUserServiceModel updateUserServiceModel = this._userMapper.Map<UpdateUserServiceModel>(updateModel);
			updateUserServiceModel.Id = id;

			UserServiceModel userServiceModel = await this._userService.UpdateUser(updateUserServiceModel);
			UserWebModel userWebModel = this._userMapper.Map<UserWebModel>(userServiceModel);

			return new AcceptedResult("UpdateUser", userWebModel);
		}
		#endregion

		#region Delete
		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			await this._userService.DeleteUser(id);
			return new OkResult();
		}
		#endregion
	}
}
