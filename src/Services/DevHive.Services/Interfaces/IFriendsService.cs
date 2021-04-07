using System;
using System.Threading.Tasks;

namespace DevHive.Services.Interfaces
{
	public interface IFriendsService
	{
		Task<object> AddFriend(Guid userId, Guid friendId);
		Task<object> RemoveFriend(Guid userId, Guid friendId);
	}
}
