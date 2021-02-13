using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Comment
{
	public class CreateCommentWebModel
	{
		[NotNull]
		[Required]
		public Guid PostId { get; set; }

		[NotNull]
		[Required]
		public string Message { get; set; }
	}
}
