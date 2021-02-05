using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Technology
{
	public class TechnologyWebModel
	{
		[NotNull]
		[Required]
		public Guid Id { get; set; }
	}
}
