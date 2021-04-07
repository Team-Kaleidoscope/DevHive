using System;
using System.Threading.Tasks;
using DevHive.Common.Constants;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;

namespace DevHive.Services.Services
{
	public class FriendsService : IFriendsService
	{
		private readonly IUserRepository _friendRepository;

		public FriendsService(IUserRepository friendRepository)
		{
			this._friendRepository = friendRepository;
		}

		public async Task<bool> AddFriend(Guid userId, Guid friendId)
		{
			User user = await this._friendRepository.GetByIdAsync(userId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, nameof(user)));

			User friend = await this._friendRepository.GetByIdAsync(friendId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, nameof(friend)));

			bool addedToUser = user.Friends.Add(friend) && await this._friendRepository.EditAsync(userId, user);
			bool addedToFriend = friend.Friends.Add(user) && await this._friendRepository.EditAsync(friendId, friend);
			return addedToUser && addedToFriend;
		}

		public async Task<bool> RemoveFriend(Guid userId, Guid friendId)
		{
			User user = await this._friendRepository.GetByIdAsync(userId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, nameof(user)));

			User friend = await this._friendRepository.GetByIdAsync(friendId) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, nameof(friend)));

			bool addedToUser = user.Friends.Remove(friend) && await this._friendRepository.EditAsync(userId, user);
			bool addedToFriend = friend.Friends.Remove(user) && await this._friendRepository.EditAsync(friendId, friend);
			return addedToUser && addedToFriend;
		}
	}
}
