using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Models.Relational
{
	[Table("RatedPosts")]
	public class RatedPost
	{
		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid PostId { get; set; }
		public Post Post { get; set; }
	}
}
