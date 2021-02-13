using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface ILanguage : IModel
	{
		string Name { get; set; }
		HashSet<User> Users { get; set; }
	}
}
