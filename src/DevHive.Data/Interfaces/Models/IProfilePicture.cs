using System;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IProfilePicture : IModel
	{
		Guid UserId { get; set; }
		User User { get; set; }

		string PictureURL { get; set; }
	}
}
