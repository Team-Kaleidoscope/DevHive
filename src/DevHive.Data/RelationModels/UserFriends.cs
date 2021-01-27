using System;
using DevHive.Data.Models;

namespace DevHive.Data.RelationModels
{
	public class UserFriends
	{
		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid FriendId { get; set; }
		public User Friend { get; set; }
	}
}
