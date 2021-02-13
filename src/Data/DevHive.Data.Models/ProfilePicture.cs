using System;
using DevHive.Data.Models.Interfaces;

namespace DevHive.Data.Models
{
	public class ProfilePicture : IProfilePicture
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }

		public string PictureURL { get; set; }
	}
}
