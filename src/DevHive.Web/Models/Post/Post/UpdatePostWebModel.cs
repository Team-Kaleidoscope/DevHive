using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Post.Post
{
	public class UpdatePostWebModel
	{
		[Required]
		[NotNull]
		public Guid PostId { get; set; }

		[NotNull]
		[Required]
		public string Message { get; set; }
	}
}
