using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Models.User;
using DevHive.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevHive.Common.Models.Identity;
using DevHive.Services.Interfaces;
using DevHive.Common.Jwt.Interfaces;
using NSwag.Annotations;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for integration with the User
	/// </summary>
	[ApiController]
	[Route("/api/[controller]")]
	[OpenApiController("User Controller")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _userMapper;
		private readonly IJwtService _jwtService;

		public UserController(IUserService userService, IMapper mapper, IJwtService jwtService)
		{
			this._userService = userService;
			this._userMapper = mapper;
			this._jwtService = jwtService;
		}

		#region Authentication
		/// <summary>
		/// Login endpoint for the DevHive Social Platform
		/// </summary>
		/// <param name="loginModel">Login model with username and password</param>
		/// <returns>A JWT Token for further validation</returns>
		[HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		[OpenApiTags("Authorization")]
		public async Task<IActionResult> Login([FromBody] LoginWebModel loginModel)
		{
			LoginServiceModel loginServiceModel = this._userMapper.Map<LoginServiceModel>(loginModel);

			TokenModel tokenModel = await this._userService.LoginUser(loginServiceModel);
			TokenWebModel tokenWebModel = this._userMapper.Map<TokenWebModel>(tokenModel);

			return new OkObjectResult(tokenWebModel);
		}

		/// <summary>
		/// Register a new User in the DevHive Social Platform
		/// </summary>
		/// <param name="registerModel">Register model with the new data to provide</param>
		/// <returns>A JWT Token for further validation</returns>
		[HttpPost]
		[AllowAnonymous]
		[Route("Register")]
		[OpenApiTag("Authorization")]
		public async Task<IActionResult> Register([FromBody] RegisterWebModel registerModel)
		{
			RegisterServiceModel registerServiceModel = this._userMapper.Map<RegisterServiceModel>(registerModel);

			TokenModel tokenModel = await this._userService.RegisterUser(registerServiceModel);
			TokenWebModel tokenWebModel = this._userMapper.Map<TokenWebModel>(tokenModel);

			return new CreatedResult("Register", tokenWebModel);
		}
		#endregion

		#region Read
		/// <summary>
		/// Get a User's information using the Guid
		/// </summary>
		/// <param name="id">User's Guid</param>
		/// <param name="authorization">The JWT Token, contained in the header and used for validation</param>
		/// <returns>A full User's read model</returns>
		[HttpGet]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> GetById(Guid id, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(id, authorization))
				return new UnauthorizedResult();

			UserServiceModel userServiceModel = await this._userService.GetUserById(id);
			UserWebModel userWebModel = this._userMapper.Map<UserWebModel>(userServiceModel);

			return new OkObjectResult(userWebModel);
		}

		/// <summary>
		/// Get a User's profile using his username. Does NOT require authorization
		/// </summary>
		/// <param name="username">User's username</param>
		/// <returns>A trimmed version of the full User's read model</returns>
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
		/// <summary>
		/// Full update on User's data. A PUSTINQK can only edit his account
		/// </summary>
		/// <param name="id">The User's Guid</param>
		/// <param name="updateUserWebModel">A full User update model</param>
		/// <param name="authorization">The JWT Token, contained in the header and used for validation</param>
		/// <returns>A full User's read model</returns>
		[HttpPut]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserWebModel updateUserWebModel, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(id, authorization))
				return new UnauthorizedResult();

			UpdateUserServiceModel updateUserServiceModel = this._userMapper.Map<UpdateUserServiceModel>(updateUserWebModel);
			updateUserServiceModel.Id = id;

			UserServiceModel userServiceModel = await this._userService.UpdateUser(updateUserServiceModel);
			UserWebModel userWebModel = this._userMapper.Map<UserWebModel>(userServiceModel);

			return new AcceptedResult("UpdateUser", userWebModel);
		}
		#endregion

		#region Delete
		/// <summary>
		/// Delete a User with his Id. A PUSTINQK can only delete his account. An Admin can delete all accounts
		/// </summary>
		/// <param name="id">The User's Guid</param>
		/// <param name="authorization">The JWT Token, contained in the header and used for validation</param>
		/// <returns>Ok, BadRequest or Unauthorized</returns>
		[HttpDelete]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Delete(Guid id, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(id, authorization))
				return new UnauthorizedResult();

			bool result = await this._userService.DeleteUser(id);
			if (!result)
				return new BadRequestObjectResult("Could not delete User");

			return new OkResult();
		}
		#endregion

		/// <summary>
		/// We don't talk about that, NIGGA!
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpPost]
		[OpenApiIgnore]
		[Authorize(Roles = "User,Admin")]
		[Route("SuperSecretPromotionToAdmin")]
		public async Task<IActionResult> SuperSecretPromotionToAdmin(Guid userId)
		{
			return new OkObjectResult(await this._userService.SuperSecretPromotionToAdmin(userId));
		}
	}
}
