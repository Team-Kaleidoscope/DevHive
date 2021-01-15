using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace DevHive.Data.Models
{
	[Table("Roles")]
	public class Role : IdentityRole<Guid>
	{
		public const string DefaultRole = "User";
		public const string AdminRole = "Admin";

		public List<User> Users { get; set; }
	}
}
