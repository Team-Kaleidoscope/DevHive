using System;

namespace DevHive.Web.Models.Post.Comment
{
	public class UpdateCommentWebModel
	{
		public Guid CommentId { get; set; }

		public string NewMessage { get; set; }
	}
}
