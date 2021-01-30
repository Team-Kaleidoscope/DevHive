using System;
using DevHive.Data.Interfaces.Models;

namespace DevHive.Data.Models
{
	public class Rating : IRating
	{
		public Guid Id { get; set; }

		public Post Post { get; set; }

		public int Likes { get; set; }

		public int Dislikes { get; set; }
	}
}
