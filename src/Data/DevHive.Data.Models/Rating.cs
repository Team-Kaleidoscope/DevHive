using System;
using System.Collections.Generic;
using DevHive.Data.Models.Interfaces;

namespace DevHive.Data.Models
{
	public class Rating : IRating
	{
		//if adding rating to comments change Post for intreface IRatable!
		public Guid Id { get; set; }

		public User User { get; set; }

		public Post Post { get; set; }

		public bool IsLike { get; set; }
	}
}
