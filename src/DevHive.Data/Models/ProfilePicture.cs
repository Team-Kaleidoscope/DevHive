using System;

namespace DevHive.Data.Models
{
	public class ProfilePicture
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }

		public string PictureURL { get; set; }
	}
}
