using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Models.Post.Post
{
	public class UpdatePostServiceModel
	{
		public Guid PostId { get; set; }

		public Guid CreatorId { get; set; }

		public string NewMessage { get; set; }

		public List<IFormFile> Files { get; set; }
	}
}
