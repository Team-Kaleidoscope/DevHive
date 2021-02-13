using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Models.Interfaces
{
	public interface ILanguage : IModel
	{
		string Name { get; set; }
		HashSet<User> Users { get; set; }
	}
}
