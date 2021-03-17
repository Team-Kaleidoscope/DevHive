using System;
using System.Collections.Generic;

namespace DevHive.Data.Models
{
	public class Technology
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public HashSet<User> Users { get; set; } = new();
	}
}
