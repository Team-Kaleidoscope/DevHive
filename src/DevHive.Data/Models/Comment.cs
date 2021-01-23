using System;
using DevHive.Data.Interfaces.Models;

namespace DevHive.Data.Models
{
	public class Comment : IComment
	{
		public Guid Id { get; set; }

		public Guid PostId { get; set; }

		public Guid CreatorId { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }
	}
}
