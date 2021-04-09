using System;
using System.ComponentModel.DataAnnotations;

namespace DevHive.Web.Models.Attributes
{
	public class OnlyLettersAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var stringValue = (string)value;

			foreach (char ch in stringValue)
			{
				if (!char.IsLetter(ch))
					return false;
			}
			return true;
		}
	}
}
