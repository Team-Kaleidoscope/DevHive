using System;
using System.Collections.Generic;
using DevHive.Data.Interfaces.Models;

namespace DevHive.Data.Models
{
	public class Rating : IRating
	{
		public Guid Id { get; set; }

		public Guid PostId { get; set; }

		public Post Post { get; set; }
		
		public int Rate { get; set; }
	}
}
