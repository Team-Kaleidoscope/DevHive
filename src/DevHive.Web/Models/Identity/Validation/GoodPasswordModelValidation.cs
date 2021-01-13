using System;
using System.ComponentModel.DataAnnotations;

namespace DevHive.Web.Models.Identity.Validation
{
	public class GoodPassword : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var stringValue = (string)value;

			for (int i = 0; i < stringValue.Length; i++)
			{
				if (Char.IsDigit(stringValue[i]))
				{
					base.ErrorMessage = "Password must be atleast 5 characters long!";
					return stringValue.Length >= 5;
				}
			}
			base.ErrorMessage = "Password must contain atleast 1 digit!";
			return false;
		}
	}
}
