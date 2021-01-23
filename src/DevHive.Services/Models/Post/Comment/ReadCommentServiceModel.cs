using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class ReadCommentServiceModel
	{
		public Guid CommentId { get; set; }

		public Guid IssuerId { get; set; }

		public Guid PostId { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
