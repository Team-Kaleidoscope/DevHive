using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class UpdateCommentServiceModel
	{
		public Guid CommentId { get; set; }

		public string NewMessage { get; set; }
	}
}
