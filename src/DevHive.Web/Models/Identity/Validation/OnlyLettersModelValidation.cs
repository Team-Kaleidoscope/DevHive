using System;
using System.ComponentModel.DataAnnotations;

namespace DevHive.Web.Models.Identity.Validation
{
	public class OnlyLetters : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var stringValue = (string)value;

			foreach (char ch in stringValue)
			{
				if (!Char.IsLetter(ch))
					return false;
			}
			return true;
		}
	}
}
