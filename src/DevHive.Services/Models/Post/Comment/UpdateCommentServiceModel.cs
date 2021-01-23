using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class UpdateCommentServiceModel
	{
		public Guid CreatorId { get; set; }

		public Guid CommentId { get; set; }

		public Guid PostId { get; set; }

		public string NewMessage { get; set; }
	}
}
