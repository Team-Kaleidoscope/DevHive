using System.Threading.Tasks;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using DevHive.Web.Models.Post.Post;
using DevHive.Services.Models.Post.Post;
using DevHive.Web.Models.Post.Comment;
using DevHive.Services.Models.Post.Comment;
using DevHive.Common.Models.Misc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	//[Authorize(Posts = "Admin")]
	public class PostController
	{
		private readonly PostService _postService;
		private readonly IMapper _postMapper;

		public PostController(PostService postService, IMapper mapper)
		{
			this._postService = postService;
			this._postMapper = mapper;
		}

		//Create
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreatePostWebModel createPostModel)
		{
			CreatePostServiceModel postServiceModel = 
				this._postMapper.Map<CreatePostServiceModel>(createPostModel); 
			
			bool result = await this._postService.CreatePost(postServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not create post!");

			return new OkResult();
		}

		[HttpPost]
		[Route("Comment")]
		public async Task<IActionResult> AddComment([FromBody] CommentWebModel commentWebModel)
		{
			CommentServiceModel commentServiceModel = this._postMapper.Map<CommentServiceModel>(commentWebModel);

			bool result = await this._postService.AddComment(commentServiceModel);

			if(!result)
				return new BadRequestObjectResult("Could not create the Comment");

			return new OkResult();
		}

		//Read
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			PostServiceModel postServiceModel = await this._postService.GetPostById(id);
			PostWebModel postWebModel = this._postMapper.Map<PostWebModel>(postServiceModel);

			return new OkObjectResult(postWebModel);
		}

		[HttpGet]
		[Route("Comment")]
		public async Task<IActionResult> GetCommentById(Guid id)
		{
			CommentServiceModel commentServiceModel = await this._postService.GetCommentById(id);
			IdModel idModel = this._postMapper.Map<IdModel>(commentServiceModel);

			return new OkObjectResult(idModel);
		}

		//Update
		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePostWebModel updatePostModel)
		{
			UpdatePostServiceModel postServiceModel = 
				this._postMapper.Map<UpdatePostServiceModel>(updatePostModel);
			postServiceModel.IssuerId = id;

			bool result = await this._postService.UpdatePost(postServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update post!");

			return new OkResult();
		}

		[HttpPut]
		[Route("Comment")]
		public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentWebModel commentWebModel, [FromHeader] string authorization)
		{
			if (!await this._postService.ValidateJwtForComment(id, authorization))
				return new UnauthorizedResult();

			UpdateCommentServiceModel updateCommentServiceModel = this._postMapper.Map<UpdateCommentServiceModel>(commentWebModel);
			updateCommentServiceModel.Id = id;

			bool result = await this._postService.UpdateComment(updateCommentServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update Comment");

			return new OkResult();
		}

		//Delete
		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._postService.DeletePost(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete post!");

			return new OkResult();
		}

		[HttpDelete]
		[Route("Comment")]
		public async Task<IActionResult> DeleteComment(Guid id, [FromHeader] string authorization)
		{
			if (!await this._postService.ValidateJwtForComment(id, authorization))
				return new UnauthorizedResult();
			
			bool result = await this._postService.DeleteComment(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete Comment");

			return new OkResult();
		}
	}
}
