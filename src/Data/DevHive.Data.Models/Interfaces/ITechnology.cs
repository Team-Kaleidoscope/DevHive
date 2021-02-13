using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Models.Interfaces
{
	public interface ITechnology : IModel
	{
		string Name { get; set; }
		HashSet<User> Users { get; set; }
	}
}
