using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Models.Interfaces
{
	public interface IRole
	{
		HashSet<User> Users { get; set; }
	}
}
