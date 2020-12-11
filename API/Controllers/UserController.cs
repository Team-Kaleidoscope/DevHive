using System.Threading.Tasks;
using API.Database;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Data.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Data.Models.Classes;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("/api/[controller]")]
	public class UserController: ControllerBase
	{
		private readonly UserService _service;

		public UserController(DevHiveContext context, IMapper mapper, IConfiguration configuration)
		{
			this._service = new UserService(context, mapper, configuration.GetSection("AppSettings"));
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
		{
			return await this._service.LoginUser(userDTO);
		}

		//Create
		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
		{
			return await this._service.CreateUser(userDTO);
		}

		//Read
		[HttpGet]
		[Authorize(Roles = UserRoles.Admin)] // Functionality, only for testing purposes
		public async Task<IActionResult> GetById(int id)
		{
			return await this._service.GetUserById(id);
		}

		//Update
		[HttpPut]
		public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDTO)
		{
			return await this._service.UpdateUser(id, userDTO);
		}

		//Delete
		[HttpDelete] 
		public async Task<IActionResult> Delete(int id)
		{
			return await this._service.DeleteUser(id);
		}
	}
}
