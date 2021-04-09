using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevHive.Data.Models
{
	[Table("ProfilePictures")]
	public class ProfilePicture
	{
		public const string DefaultURL = "/assets/icons/tabler-icon-user.svg";

		public Guid Id { get; set; }

		public Guid UserId { get; set; }
		public User User { get; set; }

		public string PictureURL { get; set; }
	}
}
