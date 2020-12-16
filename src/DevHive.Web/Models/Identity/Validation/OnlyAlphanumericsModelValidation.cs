using System;
using System.ComponentModel.DataAnnotations;

namespace DevHive.Web.Models.Identity.Validation
{
	public class OnlyAlphanumerics : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var stringValue = (string)value;

			foreach (char ch in stringValue)
			{
				if (!Char.IsLetterOrDigit(ch))
					return false;
			}
			return true;
		}
	}
}
