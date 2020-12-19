using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Identity.User;
using DevHive.Services.Options;

namespace DevHive.Services.Services
{
	public class FriendsService
	{
		private readonly FriendsRepository _friendsRepository;
		private readonly IMapper _friendsMapper;

		public FriendsService(FriendsRepository friendsRepository, IMapper mapper)
		{
			this._friendsRepository = friendsRepository;
			this._friendsMapper = mapper;
		}

		//Create
		public async Task<bool> AddFriend(Guid userId, Guid friendId)
		{
			User user = await this._friendsRepository.GetByIdAsync(userId);
			User friend = await this._friendsRepository.GetByIdAsync(friendId);

			if (DoesUserHaveThisFriend(user, friend))
				throw new ArgumentException("Friend already exists in your friends list.");

			return user != default(User) && friend != default(User) ? 
				await this._friendsRepository.AddFriendAsync(user, friend) : 
				throw new ArgumentException("Invalid user!");
		}
		
		//Read
		public async Task<UserServiceModel> GetFriendById(Guid friendId)
		{
			if(!_friendsRepository.DoesUserExist(friendId))
				throw new ArgumentException("User does not exist!");

			User friend = await this._friendsRepository.GetByIdAsync(friendId);

			return this._friendsMapper.Map<UserServiceModel>(friend);
		}
	
		public async Task<bool> RemoveFriend(Guid userId, Guid friendId)
		{
			if(!this._friendsRepository.DoesUserExist(userId) && 
				!this._friendsRepository.DoesUserExist(friendId))
					throw new ArgumentException("Invalid user!");

			User user = await this._friendsRepository.GetByIdAsync(userId);
			User friend = await this._friendsRepository.GetByIdAsync(friendId);

			if(!this.DoesUserHaveFriends(user))
				throw new ArgumentException("User does not have any friends.");

			if (!DoesUserHaveThisFriend(user, friend))
				throw new ArgumentException("This ain't your friend, amigo.");

			return await this.RemoveFriend(user.Id, friendId);
		}

		//Validation
		private bool DoesUserHaveThisFriend(User user, User friend)
		{
			return user.Friends.Contains(friend);
		}

		private bool DoesUserHaveFriends(User user)
		{
			return user.Friends.Count >= 1;
		}
	}
}