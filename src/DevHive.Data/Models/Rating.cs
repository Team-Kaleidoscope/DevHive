using System;
using System.Collections.Generic;
using DevHive.Data.Interfaces.Models;
using DevHive.Data.RelationModels;

namespace DevHive.Data.Models
{
	public class Rating : IRating
	{
		public Guid Id { get; set; }

		// public Guid PostId { get; set; }
		// public Post Post { get; set; }

		public int Rate { get; set; }

		// public HashSet<User> UsersThatRated { get; set; } = new();
	}
}
