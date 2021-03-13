using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using DevHive.Web.Models.Comment;
using DevHive.Services.Models.Comment;
using Microsoft.AspNetCore.Authorization;
using DevHive.Services.Interfaces;
using DevHive.Common.Jwt.Interfaces;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the comments layer
	/// </summary>
	[ApiController]
	[Route("/api/[controller]")]
	[Authorize(Roles = "User,Admin")]
	public class CommentController
	{
		private readonly ICommentService _commentService;
		private readonly IMapper _commentMapper;
		private readonly IJwtService _jwtService;

		public CommentController(ICommentService commentService, IMapper commentMapper, IJwtService jwtService)
		{
			this._commentService = commentService;
			this._commentMapper = commentMapper;
			this._jwtService = jwtService;
		}

		/// <summary>
		/// Create a comment and attach it to a post
		/// </summary>
		/// <param name="userId">The useer's Id</param>
		/// <param name="createCommentWebModel">The new comment's parametars</param>
		/// <param name="authorization">JWT Bearer token</param>
		/// <returns>The comment's Id</returns>
		[HttpPost]
		public async Task<IActionResult> AddComment(Guid userId, [FromBody] CreateCommentWebModel createCommentWebModel, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

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

		/// <summary>
		/// Query comment's data by it's Id
		/// </summary>
		/// <param name="commentId">The comment's Id</param>
		/// <returns>Full data model of the comment</returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetCommentById(Guid commentId)
		{
			ReadCommentServiceModel readCommentServiceModel = await this._commentService.GetCommentById(commentId);
			ReadCommentWebModel readCommentWebModel = this._commentMapper.Map<ReadCommentWebModel>(readCommentServiceModel);

			return new OkObjectResult(readCommentWebModel);
		}

		/// <summary>
		/// Update comment's parametars. Comment creator only!
		/// </summary>
		/// <param name="userId">The comment creator's Id</param>
		/// <param name="updateCommentWebModel">New comment's parametars</param>
		/// <param name="authorization">JWT Bearer token</param>
		/// <returns>Ok result</returns>
		[HttpPut]
		public async Task<IActionResult> UpdateComment(Guid userId, [FromBody] UpdateCommentWebModel updateCommentWebModel, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			UpdateCommentServiceModel updateCommentServiceModel =
				this._commentMapper.Map<UpdateCommentServiceModel>(updateCommentWebModel);
			updateCommentServiceModel.CreatorId = userId;

			Guid id = await this._commentService.UpdateComment(updateCommentServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult("Unable to update comment!") :
				new OkObjectResult(new { Id = id });
		}

		/// <summary>
		/// Delete a comment. Comment creator only!
		/// </summary>
		/// <param name="commentId">Comment's Id</param>
		/// <param name="authorization">JWT Bearer token</param>
		/// <returns>Ok result</returns>
		[HttpDelete]
		public async Task<IActionResult> DeleteComment(Guid commentId, [FromHeader] string authorization)
		{
			if (!await this._commentService.ValidateJwtForComment(commentId, authorization))
				return new UnauthorizedResult();

			return await this._commentService.DeleteComment(commentId) ?
				new OkResult() :
				new BadRequestObjectResult("Could not delete Comment");
		}
	}
}

