using System;
using System.Collections.Generic;
using DevHive.Services.Models.Post.Comment;

namespace DevHive.Services.Models.Post.Post
{
	public class ReadPostServiceModel
	{
		public Guid PostId { get; set; }

		public Guid CreatorId { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		public List<ReadCommentServiceModel> Comments { get; set; }

		//public List<string> Files { get; set; }
	}
}
