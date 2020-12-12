using System.Threading.Tasks;
using API.Database;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Data.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Data.Models.Options;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
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
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			return await this._service.LoginUser(loginDTO);
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
		{
			return await this._service.RegisterUser(registerDto);
		}

		//Read
		[HttpGet]
		public async Task<IActionResult> GetById(int id)
		{
			return await this._service.GetUserById(id);
		}

		//Update
		[HttpPut]
		[Authorize]
		public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDTO)
		{
			return await this._service.UpdateUser(id, userDTO);
		}

		//Delete
		[HttpDelete]
	   	[Authorize]	
		public async Task<IActionResult> Delete(int id)
		{
			return await this._service.DeleteUser(id);
		}
	}
}
