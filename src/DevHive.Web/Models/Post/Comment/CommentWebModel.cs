using System;

namespace DevHive.Web.Models.Post.Comment
{
	public class CommentWebModel
	{
		public Guid IssuerId { get; set; }
		public string Message { get; set; }
	}
}