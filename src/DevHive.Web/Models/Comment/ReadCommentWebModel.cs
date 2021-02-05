using System;

namespace DevHive.Web.Models.Comment
{
	public class ReadCommentWebModel
	{
		public Guid CommentId { get; set; }

		public Guid PostId { get; set; }

		public string IssuerFirstName { get; set; }

		public string IssuerLastName { get; set; }

		public string IssuerUsername { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
