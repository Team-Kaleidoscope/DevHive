using System;
using System.Threading.Tasks;
using DevHive.Data.Repositories;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class RoleController
	{
		private readonly RoleService _service;

		public RoleController(DevHiveContext context)
		{
			//this._service = new RoleService(context);
		}

		[HttpPost]
		public Task<IActionResult> Create(string name)
		{
			//return this._service.CreatePost(name);
			throw new NotImplementedException();
		}

		[HttpGet]
		public Task<IActionResult> ShowPost(uint postId)
		{
			//return this._service.GetPostById(postId);
			throw new NotImplementedException();
		}
	}
}
