using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Interfaces.Models;

namespace DevHive.Data.Models
{
	[Table("Posts")]
	public class Post : IPost
	{
		public Guid Id { get; set; }

		public User Creator { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		public List<Comment> Comments { get; set; } = new();

		public Rating Rating { get; set; } = new();

		public List<string> FileUrls { get; set; } = new();
	}
}
