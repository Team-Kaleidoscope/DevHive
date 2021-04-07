using System;
using System.Threading.Tasks;

namespace DevHive.Services.Interfaces
{
	public interface IFriendsService
	{
		Task<bool> AddFriend(Guid userId, Guid friendId);
		Task<bool> RemoveFriend(Guid userId, Guid friendId);
	}
}
