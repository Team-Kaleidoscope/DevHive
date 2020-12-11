using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Database;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Data.Models.Classes;
using Data.Models.DTOs;

namespace API.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class UserController: ControllerBase
	{
		private readonly UserService _service;

		public UserController(DevHiveContext context, IMapper mapper)
		{
			this._service = new UserService(context, mapper);
		}

		//Create
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] UserDTO userDTO)
		{
			return await this._service.CreateUser(userDTO);
		}

		//Read
		[HttpGet]
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
