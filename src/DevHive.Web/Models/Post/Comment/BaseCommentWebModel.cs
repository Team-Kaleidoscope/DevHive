using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Post.Comment
{
	public class BaseCommentWebModel
	{
		[NotNull]
		[Required]
		public Guid PostId { get; set; }

		[NotNull]
		[Required]
		public Guid CommentId { get; set; }

		[NotNull]
		[Required]
		public string Message { get; set; }
	}
}
