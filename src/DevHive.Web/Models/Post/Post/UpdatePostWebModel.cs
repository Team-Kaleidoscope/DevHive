using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Post.Post
{
	public class UpdatePostWebModel
	{
		[NotNull]
		[Required]
		public string Message { get; set; }
	}
}
