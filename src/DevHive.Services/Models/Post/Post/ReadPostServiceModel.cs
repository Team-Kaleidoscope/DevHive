using System;
using System.Collections.Generic;
using DevHive.Services.Models.Post.Comment;
using Microsoft.Extensions.FileProviders;

namespace DevHive.Services.Models.Post.Post
{
	public class ReadPostServiceModel
	{
		public Guid PostId { get; set; }

		public string CreatorFirstName { get; set; }

		public string CreatorLastName { get; set; }

		public string CreatorUsername { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		public List<ReadCommentServiceModel> Comments { get; set; } = new();

		public List<IFileInfo> Files { get; set; }
	}
}
