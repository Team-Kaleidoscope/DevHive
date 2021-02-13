using System;
using DevHive.Data.Models;

namespace DevHive.Data.Models.Interfaces
{
	public interface IProfilePicture : IModel
	{
		Guid UserId { get; set; }
		User User { get; set; }

		string PictureURL { get; set; }
	}
}
