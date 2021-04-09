using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System;

namespace DevHive.Data.Models
{
	[Table("Roles")]
	public class Role : IdentityRole<Guid>
	{
		public const string DefaultRole = "User";
		public const string AdminRole = "Admin";

		public HashSet<User> Users { get; set; } = new();
	}
}
