using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Interfaces.Models;
using Microsoft.AspNetCore.Identity;

namespace DevHive.Data.Models
{
	[Table("Users")]
	public class User : IdentityUser<Guid>, IUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string ProfilePictureUrl { get; set; }

		/// <summary>
		/// Languages that the user uses or is familiar with
		/// </summary>
		// [Unique]
		public IList<Language> Languages { get; set; }

		/// <summary>
		/// Technologies that the user uses or is familiar with
		/// </summary>
		public IList<Technology> Technologies { get; set; } = new List<Technology>();

		public IList<Role> Roles { get; set; } = new List<Role>();

		public IList<User> Friends { get; set; } = new List<User>();
	}
}
