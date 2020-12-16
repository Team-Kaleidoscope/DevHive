using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevHive.Data.Models
{
	[Table("Users")]
	public class User : IdentityUser<Guid>, IModel
	{
		public override string UserName
		{
			get => base.UserName;
			set => base.UserName = value;
		}

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public string Role { get; set; }

		//public List<User> Friends { get; set; }
	}
}
