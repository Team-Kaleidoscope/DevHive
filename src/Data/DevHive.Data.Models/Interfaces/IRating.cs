using System;
using System.Collections.Generic;
using DevHive.Data.Models;

namespace DevHive.Data.Models.Interfaces
{
	public interface IRating : IModel
	{
		bool IsLike { get; set; }

		Post Post { get; set; }

		User User { get; set; }
	}
}
