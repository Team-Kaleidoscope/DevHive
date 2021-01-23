using System;
using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IPost : IModel
	{
		Guid CreatorId { get; set; }

		string Message { get; set; }

		DateTime TimeCreated { get; set; }

		List<Comment> Comments { get; set; }

		//List<Files> Files
	}
}
