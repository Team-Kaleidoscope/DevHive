using System;

namespace DevHive.Services.Models.Post.Post
{
	public class CreatePostServiceModel
	{
		public Guid CreatorId { get; set; }

		public string Message { get; set; }

		// public List<IFormFile> Files { get; set; }
	}
}
