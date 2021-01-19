using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Post.Post
{
	public class BasePostWebModel
	{
		[NotNull]
		[Required]
		public Guid IssuerId { get; set; }

		[NotNull]
		[Required]
		public string Message { get; set; }
	}
}
