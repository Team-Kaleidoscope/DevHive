using System;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IPost : IModel
	{
		Guid IssuerId { get; set; }
		DateTime TimeCreated { get; set; }
		string Message { get; set; }
		Comment[] Comments { get; set; }
	}
}
