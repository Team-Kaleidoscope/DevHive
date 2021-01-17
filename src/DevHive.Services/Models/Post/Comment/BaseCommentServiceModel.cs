using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class BaseCommentServiceModel
	{
		public Guid Id { get; set; }
		public Guid PostId { get; set; }
		public Guid IssuerId { get; set; }
		public string Message { get; set; }
	}
}
