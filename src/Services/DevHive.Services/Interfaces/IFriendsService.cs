using System;
using System.Threading.Tasks;

namespace DevHive.Services.Interfaces
{
	public interface IFriendsService
	{
		Task<bool> AddFriend(Guid userId, string friendUsername);
		Task<bool> RemoveFriend(Guid userId, string friendUsername);
	}
}
