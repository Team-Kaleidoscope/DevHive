using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Models.Interfaces;
using DevHive.Data.Models.Relational;
using Microsoft.AspNetCore.Identity;

namespace DevHive.Data.Models
{
	[Table("Users")]
	public class User : IdentityUser<Guid>, IUser
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public ProfilePicture ProfilePicture { get; set; }

		public HashSet<Language> Languages { get; set; } = new();

		public HashSet<Technology> Technologies { get; set; } = new();

		public HashSet<Role> Roles { get; set; } = new();

		public HashSet<Post> Posts { get; set; } = new();

		public HashSet<User> Friends { get; set; } = new();

		public HashSet<Comment> Comments { get; set; } = new();

		public HashSet<RatedPost> RatedPosts { get; set; } = new();
	}
}
