using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Technology
{
	public class UpdateTechnologyWebModel
	{
		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Name { get; set; }
	}
}
