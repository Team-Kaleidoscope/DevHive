using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Data.Models.Classes
{
	[Table("Users")]
	public class User : IdentityUser<int>
	{
		private string _firstName;
		private string _lastName;
		private string _profilePicture;

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

		public string ProfilePicture 
		{
			get => this._profilePicture;
			set 
			{
				ValidateURL(value);
				this._profilePicture = value;
			}
		}
	
		public string Role { get; set; }

		// public List<User> Friends { get; set; }

		/// <summary>
		/// Throws an argument exception if the given value is not composed only of letters, and if specified, also of digits.
		/// Does nothing otherwise.
		/// </summary>
		private static void ValidateString(string name, int minLength, int maxLength, string value, bool canBeDigit) 
		{
			if (value.Length < minLength || value.Length > maxLength)
				throw new ArgumentException($"{name} length cannot be less than {minLength} and more than {maxLength}.");

			foreach (char character in value) // more efficient than Linq
			{ 	
				if (!Char.IsLetter(character))
					if (!(Char.IsDigit(character) && canBeDigit))
						throw new ArgumentException($"{name} contains invalid characters."); 
			}
		}

		/// <summary>
		/// Throws an exception if the absolute url isn't valid.
		/// Does nothing otherwise.
		/// </summary>
		private static void ValidateURL(string urlValue) 
		{
			// Throws an error is URL is invalid
			Uri validatedUri;
			Uri.TryCreate(urlValue, UriKind.Absolute, out validatedUri);
		}
	}
}
