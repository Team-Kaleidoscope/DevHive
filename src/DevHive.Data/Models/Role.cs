using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevHive.Data.Models
{
	[Table("Roles")]
	public class Role : IdentityRole<Guid>
	{
		public const string DefaultRole = "User";
	}
}
