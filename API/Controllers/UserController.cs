using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Database;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Classes;
using Models.DTOs;

namespace API.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class UserController: ControllerBase
	{
		private readonly UserService _service;

		public UserController(DevHiveContext context, Mapper mapper)
		{
			this._service = new UserService(context, mapper);
		}

		//Create
		[HttpPost]
		public async Task<HttpStatusCode> Create([FromBody] UserDTO userDTO)
		{
			return await this._service.CreateUser(userDTO);
		}

		//Read
		[HttpGet]
		public async Task<string> GetById(int id)
		{
			return await this._service.GetUserById(id);
		}

		//Update
		[HttpPut]
		public async Task<HttpStatusCode> Update(int id, [FromBody] UserDTO userDTO)
		{
			return await this._service.UpdateUser(id, userDTO);
		}

		//Delete
		[HttpDelete] 
		public async Task<HttpStatusCode> Delete(int id)
		{
			return await this._service.DeleteUser(id);
		}
	}
}
