using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Database;
using API.Service;
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

		public UserController(DevHiveContext context)
		{
			this._service = new UserService(context);
		}

		//Create
		[HttpPost]
		public async Task<HttpStatusCode> Create([FromForm] UserDTO userDTO)
		{
			HttpStatusCode returnStatusCode = await this._service.CreateUser(userDTO);

			return returnStatusCode;
		}

		//Read
		// [HttpGet]

		// //Update
		// [HttpPut]

		// //Delete
		// [HttpDelete]
	}
}