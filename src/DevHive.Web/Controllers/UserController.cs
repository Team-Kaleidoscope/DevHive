using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Repositories;
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
		private readonly UserService _service;

		public UserController(DevHiveContext context, IMapper mapper, JWTOptions jwtOptions)
		{
			this._service = new UserService(context, mapper, jwtOptions);
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginWebModel loginWebModel)
		{
			var loginDTO = 
			return await this._service.LoginUser(loginDTO);
			//throw new NotImplementedException();
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterWebModel registerWebModel)
		{
			//return await this._service.RegisterUser(registerDto);
			throw new NotImplementedException();
		}

		//Read
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			//return await this._service.GetUserById(id);
			throw new NotImplementedException();
		}

		//Update
		[HttpPut]
		[Authorize]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserWebModel updateUserWebModel)
		{
			//return await this._service.UpdateUser(id, userDTO);
			throw new NotImplementedException();
		}

		//Delete
		[HttpDelete]
	   	[Authorize]	
		public async Task<IActionResult> Delete(Guid id)
		{
			//return await this._service.DeleteUser(id);
			throw new NotImplementedException();
		}
	}
}
