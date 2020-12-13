using System;
using System.Threading.Tasks;
using API.Database;
using Microsoft.AspNetCore.Mvc;

namespace API.Service
{
	public class RoleService
	{
		private readonly DevHiveContext _context;

		public RoleService(DevHiveContext context)
		{
			this._context = context;
		}

		public Task<IActionResult> CreatePost(string name)
		{
			throw new NotImplementedException();
		}

		public Task<IActionResult> GetPostById(uint postId)
		{
			throw new NotImplementedException();
		}
	}
}
