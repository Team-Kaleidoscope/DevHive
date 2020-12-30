using System;

namespace DevHive.Web.Models.Comment
{
	public class CommentWebModel
	{
		public Guid UserId { get; set; }
		public string Message { get; set; }
	}
}