using System;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IComment : IModel
	{
		Post Post { get; set; }

		User Creator { get; set; }

		string Message { get; set; }

		DateTime TimeCreated { get; set; }
	}
}
