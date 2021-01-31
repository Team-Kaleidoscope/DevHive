using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using DevHive.Web.Models.Comment;
using DevHive.Services.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using DevHive.Services.Interfaces;

namespace DevHive.Web.Controllers
{
    [ApiController]
	[Route("/api/[controller]")]
	[Authorize(Roles = "User,Admin")]
	public class CommentController {
		private readonly ICommentService _commentService;
		private readonly IMapper _commentMapper;

		public CommentController(ICommentService commentService, IMapper commentMapper)
		{
			this._commentService = commentService;
			this._commentMapper = commentMapper;
		}

		[HttpPost]
		public async Task<IActionResult> AddComment(Guid userId, [FromBody] CreateCommentWebModel createCommentWebModel, [FromHeader] string authorization)
		{
			if (!await this._commentService.ValidateJwtForCreating(userId, authorization))
				return new UnauthorizedResult();

			CreateCommentServiceModel createCommentServiceModel =
				this._commentMapper.Map<CreateCommentServiceModel>(createCommentWebModel);
			createCommentServiceModel.CreatorId = userId;

			Guid id = await this._commentService.AddComment(createCommentServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult("Could not create comment!") :
				new OkObjectResult(new { Id = id });
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetCommentById(Guid id)
		{
			ReadCommentServiceModel readCommentServiceModel = await this._commentService.GetCommentById(id);
			ReadCommentWebModel readCommentWebModel = this._commentMapper.Map<ReadCommentWebModel>(readCommentServiceModel);

			return new OkObjectResult(readCommentWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateComment(Guid userId, [FromBody] UpdateCommentWebModel updateCommentWebModel, [FromHeader] string authorization)
		{
			if (!await this._commentService.ValidateJwtForComment(updateCommentWebModel.CommentId, authorization))
				return new UnauthorizedResult();

			UpdateCommentServiceModel updateCommentServiceModel =
				this._commentMapper.Map<UpdateCommentServiceModel>(updateCommentWebModel);
			updateCommentServiceModel.CreatorId = userId;

			Guid id = await this._commentService.UpdateComment(updateCommentServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult("Unable to update comment!") :
				new OkObjectResult(new { Id = id });
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteComment(Guid id, [FromHeader] string authorization)
		{
			if (!await this._commentService.ValidateJwtForComment(id, authorization))
				return new UnauthorizedResult();

			return await this._commentService.DeleteComment(id) ?
				new OkResult() :
				new BadRequestObjectResult("Could not delete Comment");
		}

	}
}

