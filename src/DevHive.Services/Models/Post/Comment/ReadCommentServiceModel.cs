using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class ReadCommentServiceModel
	{
		public Guid CommentId { get; set; }

		public string IssuerFirstName { get; set; }

		public string IssuerLastName { get; set; }

		public string IssuerUsername { get; set; }

		public Guid PostId { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
