using System;

namespace DevHive.Data.Models
{
	public class Comment
	{
		public Guid Id { get; set; }

		public Post Post { get; set; }

		public User Creator { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
