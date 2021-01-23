using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class CreateCommentServiceModel
	{
		public Guid PostId { get; set; }

		public Guid IssuerId { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
