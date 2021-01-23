using System;
using System.Collections.Generic;
using DevHive.Web.Models.Post.Comment;

namespace DevHive.Web.Models.Post.Post
{
	public class ReadPostWebModel
	{
		public Guid PostId { get; set; }

		public string CreatorFirstName { get; set; }

		public string CreatorLastName { get; set; }

		public string CreatorUsername { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		public List<ReadCommentWebModel> Comments { get; set; }

		//public Files[] Files { get; set; }
	}
}
