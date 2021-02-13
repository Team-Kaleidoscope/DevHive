using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Models.Interfaces
{
	public interface IRating : IModel
	{
		// Post Post { get; set; }

		int Rate { get; set; }

		// HashSet<User> UsersThatRated { get; set; }
	}
}
