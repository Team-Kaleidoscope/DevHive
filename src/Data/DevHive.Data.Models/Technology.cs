using System;
using System.Collections.Generic;
using DevHive.Data.Models.Interfaces;

namespace DevHive.Data.Models
{
	public class Technology : ITechnology
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public HashSet<User> Users { get; set; } = new();
	}
}
