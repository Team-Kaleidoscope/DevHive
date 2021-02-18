using System;

namespace DevHive.Data.Models
{
	public class Message
	{
		public User Creator { get; set; }

		public string Content { get; set; }

		public DateTime TimeCreated { get; set; }

		public Chat Chat { get; set; }
	}
}
