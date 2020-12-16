using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevHive.Data.Models
{
	[Table("Users")]
	public class User : IdentityUser<Guid>, IModel
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		[Display(Name = "Username")]
		public override string UserName
		{
			get => base.UserName;
			set => base.UserName = value;
		}

		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		public string FirstName { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public string Role { get; set; }

		//public List<User> Friends { get; set; }
	}
}
