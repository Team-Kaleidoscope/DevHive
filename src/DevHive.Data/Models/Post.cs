using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevHive.Data.Models
{
	[Table("Posts")]
	public class Post
	{
		public Guid Id { get; set; }

		public Guid IssuerId { get; set; }

		public DateTime TimeCreated { get; set; }

		public string Message { get; set; }

		//public File[] Files { get; set; }

		public Comment[] Comments { get; set; }
	}
}
