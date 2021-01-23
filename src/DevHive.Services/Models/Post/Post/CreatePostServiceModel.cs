using System;

namespace DevHive.Services.Models.Post.Post
{
	public class CreatePostServiceModel
	{
		public Guid IssuerId { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		// public List<IFormFile> Files { get; set; }
	}
}
