using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Post.Post
{
	public class CreatePostWebModel
	{
		[NotNull]
		[Required]
		public string Message { get; set; }

		// public List<IFormFile> Files { get; set; }
	}
}
