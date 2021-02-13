using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Language
{
	public class LanguageWebModel
	{
		[NotNull]
		[Required]
		public Guid Id { get; set; }
	}
}
