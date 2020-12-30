using System;

namespace DevHive.Web.Models.Post.Post
{
	public class BasePostWebModel
	{
		public Guid IssuerId { get; set; }
		public string Message { get; set; }
	}
}