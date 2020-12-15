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

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class UserController: ControllerBase
	{
		private readonly UserService _userService;
		private readonly IMapper _userMapper;

		public UserController(DevHiveContext context, IMapper mapper, JWTOptions jwtOptions)
		{
			this._userService = new UserService(context, mapper, jwtOptions);
			this._userMapper = mapper;
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginWebModel loginModel)
		{
			LoginServiceModel loginServiceModel = this._userMapper.Map<LoginServiceModel>(loginModel);

			return await this._userService.LoginUser(loginServiceModel);
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterWebModel registerModel)
		{
			RegisterServiceModel registerServiceModel = this._userMapper.Map<RegisterServiceModel>(registerModel);

			return await this._userService.RegisterUser(registerServiceModel);
		}

		//Read
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			return await this._userService.GetUserById(id);
		}

		//Update
		[HttpPut]
		[Authorize]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserWebModel updateModel)
		{
			UpdateUserServiceModel updateUserServiceModel = this._userMapper.Map<UpdateUserServiceModel>(updateModel);
			updateUserServiceModel.Id = id;

			return await this._userService.UpdateUser(updateUserServiceModel);
		}

		//Delete
		[HttpDelete]
	   	[Authorize]	
		public async Task<IActionResult> Delete(Guid id)
		{
			return await this._userService.DeleteUser(id);
		}
	}
}
