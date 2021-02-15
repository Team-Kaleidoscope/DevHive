using System;
using DevHive.Data.Models.Interfaces;

namespace DevHive.Data.Models
{
	public class Comment : IComment
	{
		public Guid Id { get; set; }

		// public Guid PostId { get; set; }

		public Post Post { get; set; }

		// public Guid CreatorId { get; set; }

		public User Creator { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
