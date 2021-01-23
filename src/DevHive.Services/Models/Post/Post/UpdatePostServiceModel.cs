using System;

namespace DevHive.Services.Models.Post.Post
{
	public class UpdatePostServiceModel
	{
		public Guid PostId { get; set; }

		public string NewMessage { get; set; }

		// public List<IFormFile> Files { get; set; }
	}
}
