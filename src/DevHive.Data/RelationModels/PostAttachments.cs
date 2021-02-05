using System;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Models;

namespace DevHive.Data.RelationModels
{
	[Table("PostAttachments")]
	public class PostAttachments
	{
		public Guid Id { get; set; }

		public Post Post { get; set; }

		public string FileUrl { get; set; }
	}
}
