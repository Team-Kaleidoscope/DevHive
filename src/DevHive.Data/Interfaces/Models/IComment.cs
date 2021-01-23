using System;

namespace DevHive.Data.Interfaces.Models
{
	public interface IComment : IModel
	{
		Guid PostId { get; set; }

		Guid CreatorId { get; set; }

		string Message { get; set; }

		DateTime TimeCreated { get; set; }
	}
}
