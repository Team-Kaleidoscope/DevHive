using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevHive.Data.Models
{
	[Table("ProfilePictures")]
	public class ProfilePicture
	{
		public Guid Id { get; set; }

		public string PictureURL { get; set; }
	}
}
