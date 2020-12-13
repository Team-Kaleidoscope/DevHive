using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.Database;
using API.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class RoleController
	{
		private readonly RoleService _service;

		public RoleController(DevHiveContext context)
		{
			this._service = new RoleService(context);
		}

		[HttpPost]
		public Task<IActionResult> Create(string name)
		{
			return this._service.CreatePost(name);
		}

		[HttpGet]
		public Task<IActionResult> ShowPost(uint postId)
		{
			return this._service.GetPostById(postId);
		}
	}
}
