using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Services.Services;
using DevHive.Web.Models.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageController : ControllerBase
	{
		private readonly MessageService _messageService;

		public MessageController(MessageService messageService)
		{
			this._messageService = messageService;
		}

		[HttpPost]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Create(Guid userId, [FromBody] CreateMessageWebModel createMessageWebModel, [FromHeader] string authorization)
		{
			throw new NotImplementedException();
		}

		[HttpGet]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Read(Guid id)
		{
			throw new NotImplementedException();
		}

		[HttpPut]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Update(Guid userId, [FromBody] object updateMessageWebModel, [FromHeader] string authorization)
		{
			//TODO: Authorize user
			throw new NotImplementedException();
		}

		[HttpDelete]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Delete(Guid id, [FromHeader] string authorization)
		{
			//TODO: Authorize user
			throw new NotImplementedException();
		}
	}
}
