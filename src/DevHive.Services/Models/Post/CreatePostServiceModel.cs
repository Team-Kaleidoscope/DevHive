using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Models.Post
{
	public class CreatePostServiceModel
	{
		public Guid CreatorId { get; set; }

		public string Message { get; set; }

		public List<IFormFile> Files { get; set; }
	}
}
