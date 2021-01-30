using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Interfaces.Models;
using DevHive.Data.RelationModels;
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
		public HashSet<Language> Languages { get; set; } = new();

		/// <summary>
		/// Technologies that the user uses or is familiar with
		/// </summary>
		public HashSet<Technology> Technologies { get; set; } = new();

		public HashSet<Role> Roles { get; set; } = new();

		public HashSet<UserFriends> Friends { get; set; } = new();

		public HashSet<Post> Posts { get; set; } = new();

		public HashSet<Comment> Comments { get; set; } = new();
	}
}
