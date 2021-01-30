using System;
using System.ComponentModel.DataAnnotations;
using DevHive.Data.Models;

namespace DevHive.Data.RelationModels
{
	public class UserFriends
	{
		[Key]
		public Guid UserId { get; set; }
		public User User { get; set; }

		[Key]
		public Guid FriendId { get; set; }
		public User Friend { get; set; }
	}
}
