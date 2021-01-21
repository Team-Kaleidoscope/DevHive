using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Post.Comment
{
	public class CommentWebModel
	{
		[NotNull]
		[Required]
		public Guid IssuerId { get; set; }

		[NotNull]
		[Required]
		public Guid PostId { get; set; }

		[NotNull]
		[Required]
		public string Message { get; set; }

		[NotNull]
		[Required]
		public DateTime TimeCreated { get; set; }
	}
}
