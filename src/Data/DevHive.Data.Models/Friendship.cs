using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevHive.Data.Models
{
	//class for handling the many to many relationship betwen users and users for friends
	public class Friendship
	{
		[NotMapped]
		public Guid BaseUserId { get; set; }
		[NotMapped]
		public Guid FriendUserId { get; set; }

		public User BaseUser { get; set; }
		public User FriendUser { get; set; }
	}
}
