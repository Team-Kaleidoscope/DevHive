using System;

namespace DevHive.Services.Models.User
{
	public class FriendshipServiceModel
	{
		public Guid BaseUserId { get; set; }

		public Guid FriendUserId { get; set; }
	}
}
