using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Comment;
using DevHive.Services.Services;
using DevHive.Web.Models.Comment;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class CommentController
	{
		private readonly CommentService _commentService;
		private readonly IMapper _commentMapper;

		public CommentController(CommentService commentService, IMapper mapper)
		{
			this._commentService = commentService;
			this._commentMapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CommentWebModel commentWebModel)
		{
			CommentServiceModel commentServiceModel = this._commentMapper.Map<CommentServiceModel>(commentWebModel);

			bool result = await this._commentService.CreateComment(commentServiceModel);

			if(!result)
				return new BadRequestObjectResult("Could not create the Comment");

			return new OkResult();
		}

		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			GetByIdCommentServiceModel getByIdCommentServiceModel = await this._commentService.GetCommentById(id);
			GetByIdCommentWebModel getByIdCommentWebModel = this._commentMapper.Map<GetByIdCommentWebModel>(getByIdCommentServiceModel);

			return new OkObjectResult(getByIdCommentWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] CommentWebModel commentWebModel)
		{
			UpdateCommentServiceModel updateCommentServiceModel = this._commentMapper.Map<UpdateCommentServiceModel>(commentWebModel);
			updateCommentServiceModel.Id = id;

			bool result = await this._commentService.UpdateComment(updateCommentServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update Comment");

			return new OkResult();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._commentService.DeleteComment(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete Comment");

			return new OkResult();
		}
	}
}