using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IRole
	{
		List<User> Users { get; set; }
	}
}
