using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DevHive.Data.Models
{
	[Table("Users")]
	public class User : IdentityUser<Guid>, IModel
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string ProfilePicture { get; set; }

		public virtual IList<Role> Roles { get; set; } = new List<Role>();

		public IList<User> Friends { get; set; } = new List<User>();
	}
}
