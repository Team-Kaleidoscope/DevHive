using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevHive.Data.Models;

namespace DevHive.Data.RelationModels
{
	[Table("UserFriends")]
	public class UserFriend
	{
		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid FriendId { get; set; }
		public User Friend { get; set; }
	}
}
