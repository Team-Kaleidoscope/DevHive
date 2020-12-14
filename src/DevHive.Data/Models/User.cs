using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevHive.Data.Models
{
	[Table("Users")]
	public class User : IdentityUser<Guid>, IModel
	{
		private string _firstName;
		private string _lastName;

		[Required]
		[Range(3, 50)]
		[Display(Name = "Username")]
		public override string UserName
		{
			get => base.UserName;
			set
			{
				ValidateString("Username", 3, 50, value, true);
				base.UserName = value;
			}
		}

		[Required]
		[Range(3, 30)]
		public string FirstName
		{
			get => this._firstName;
			set
			{
				ValidateString("FirstName", 3, 30, value, false);
				this._firstName = value;
			}
		}

		[Required]
		[Range(3, 30)]
		public string LastName
		{
			get => this._lastName;
			set
			{
				ValidateString("LastName", 3, 30, value, false);
				this._lastName = value;
			}
		}

		public string ProfilePicture { get; set; }

		public string Role { get; set; }

		//public List<User> Friends { get; set; }

		private static void ValidateString(string propertyName, int minLength, int maxLength, string value, bool canBeDigit)
		{
			if (value.Length < minLength || value.Length > maxLength)
				throw new ArgumentException($"{propertyName} length cannot be less than {minLength} and more than {maxLength}.");

			foreach (char ch in value)
				if (!Char.IsLetter(ch) && !(Char.IsDigit(ch) && canBeDigit))
					throw new ArgumentException($"{propertyName} contains invalid characters.");
		}
	}
}
