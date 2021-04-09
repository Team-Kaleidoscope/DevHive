using System.ComponentModel.DataAnnotations;

namespace DevHive.Web.Models.Attributes
{
	public class GoodPasswordAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var stringValue = (string)value;

			for (int i = 0; i < stringValue.Length; i++)
			{
				if (char.IsDigit(stringValue[i]))
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
