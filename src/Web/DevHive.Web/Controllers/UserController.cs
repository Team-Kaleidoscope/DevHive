using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Models.User;
using DevHive.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevHive.Common.Models.Identity;
using DevHive.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
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
		[Authorize(Roles = "User,Admin")]
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
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserWebModel updateUserWebModel, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			UpdateUserServiceModel updateUserServiceModel = this._userMapper.Map<UpdateUserServiceModel>(updateUserWebModel);
			updateUserServiceModel.Id = id;

			UserServiceModel userServiceModel = await this._userService.UpdateUser(updateUserServiceModel);
			UserWebModel userWebModel = this._userMapper.Map<UserWebModel>(userServiceModel);

			return new AcceptedResult("UpdateUser", userWebModel);
		}

		[HttpPut]
		[Route("ProfilePicture")]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> UpdateProfilePicture(Guid userId, [FromForm] UpdateProfilePictureWebModel updateProfilePictureWebModel, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(userId, authorization))
				return new UnauthorizedResult();

			UpdateProfilePictureServiceModel updateProfilePictureServiceModel = this._userMapper.Map<UpdateProfilePictureServiceModel>(updateProfilePictureWebModel);
			updateProfilePictureServiceModel.UserId = userId;

			ProfilePictureServiceModel profilePictureServiceModel = await this._userService.UpdateProfilePicture(updateProfilePictureServiceModel);
			ProfilePictureWebModel profilePictureWebModel = this._userMapper.Map<ProfilePictureWebModel>(profilePictureServiceModel);

			return new AcceptedResult("UpdateProfilePicture", profilePictureWebModel);
		}
		#endregion

		#region Delete
		[HttpDelete]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Delete(Guid id, [FromHeader] string authorization)
		{
			if (!await this._userService.ValidJWT(id, authorization))
				return new UnauthorizedResult();

			bool result = await this._userService.DeleteUser(id);
			if (!result)
				return new BadRequestObjectResult("Could not delete User");

			return new OkResult();
		}
		#endregion

		[HttpPost]
		[Authorize(Roles = "User,Admin")]
		[Route("SuperSecretPromotionToAdmin")]
		public async Task<IActionResult> SuperSecretPromotionToAdmin(Guid userId)
		{
			return new OkObjectResult(await this._userService.SuperSecretPromotionToAdmin(userId));
		}
	}
}
