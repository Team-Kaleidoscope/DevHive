using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IUser : IModel
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string ProfilePictureUrl { get; set; }
		IList<Language> Langauges { get; set; }
		IList<Technology> Technologies { get; set; }
		IList<Role> Roles { get; set; }
		IList<User> Friends { get; set; }
	}
}
