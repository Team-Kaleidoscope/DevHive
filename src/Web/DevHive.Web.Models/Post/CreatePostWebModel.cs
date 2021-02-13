using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace DevHive.Web.Models.Post
{
	public class CreatePostWebModel
	{
		[NotNull]
		[Required]
		public string Message { get; set; }

		public List<IFormFile> Files { get; set; }
	}
}
