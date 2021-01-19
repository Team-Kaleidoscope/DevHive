using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface ILanguage : IModel
	{
		string Name { get; set; }
		List<User> Users { get; set; }

	}
}
