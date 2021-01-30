using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace DevHive.Web.Models.Post
{
	public class UpdatePostWebModel
	{
		[Required]
		[NotNull]
		public Guid PostId { get; set; }

		[NotNull]
		[Required]
		public string NewMessage { get; set; }

		public List<IFormFile> Files { get; set; } = new();
	}
}
