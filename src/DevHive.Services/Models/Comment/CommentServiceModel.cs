using System;

namespace DevHive.Services.Models.Comment
{
	public class CommentServiceModel
	{
		public Guid UserId { get; set; }
		public string Message { get; set; }
	}
}