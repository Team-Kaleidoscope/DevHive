using System;

namespace DevHive.Web.Models.Comment
{
	public class UpdateCommentWebModel
	{
		public Guid CommentId { get; set; }

		public Guid PostId { get; set; }

		public string NewMessage { get; set; }
	}
}
