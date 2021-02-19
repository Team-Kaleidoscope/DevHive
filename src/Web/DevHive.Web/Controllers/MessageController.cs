using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Models.Message;
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
		private readonly IMapper _messageMapper;

		public MessageController(MessageService messageService, IMapper messageMapper)
		{
			this._messageService = messageService;
			this._messageMapper = messageMapper;
		}

		[HttpPost]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Create(Guid userId, [FromBody] CreateMessageWebModel createMessageWebModel, [FromHeader] string authorization)
		{
			if (!await this._messageService.ValidateJwtForCreating(userId, authorization))
				return new UnauthorizedResult();

			CreateMessageServiceModel createMessageServiceModel = this._messageMapper.Map<CreateMessageServiceModel>(createMessageWebModel);

			Guid messageId = await this._messageService.CreateMessage(createMessageServiceModel);

			if (messageId == Guid.Empty)
				return BadRequest();

			return new OkObjectResult(messageId);
		}

		[HttpGet]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> Read(Guid id)
		{
			ReadMessageServiceModel readMessageServiceModel = await this._messageService.GetMessageById(id);
			if (readMessageServiceModel == null)
				return BadRequest("Message does not exist!");

			ReadMessageWebModel readMessageWebModel = this._messageMapper.Map<ReadMessageWebModel>(readMessageServiceModel);

			return new OkObjectResult(readMessageWebModel);
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
