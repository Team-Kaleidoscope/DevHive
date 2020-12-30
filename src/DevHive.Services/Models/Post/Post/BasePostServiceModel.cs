using System;

namespace DevHive.Services.Models.Post.Post
{
	public class BasePostServiceModel
	{
		public Guid Id { get; set; }
		public Guid IssuerId { get; set; }
		public string Message { get; set; }
	}
}