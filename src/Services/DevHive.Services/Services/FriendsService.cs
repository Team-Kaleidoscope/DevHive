using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.User;

namespace DevHive.Services.Services
{
	public class FriendsService : IFriendsService
	{
		private readonly IUserService _userService;
		private readonly IMapper _userMapper;

		public FriendsService(IUserService userService,
			IMapper mapper)
		{
			this._userService = userService;
			this._userMapper = mapper;
		}

		public async Task<bool> AddFriend(Guid userId, Guid friendId)
		{
			UserServiceModel user = await this._userService.GetUserById(userId);
			UserServiceModel friendUser = await this._userService.GetUserById(friendId);

			UpdateUserServiceModel updateUser = this._userMapper.Map<UpdateUserServiceModel>(user);
			UpdateFriendServiceModel updatefriendUser = this._userMapper.Map<UpdateFriendServiceModel>(friendUser);

			updateUser.Friends.Add(updatefriendUser);

			return (await this._userService.UpdateUser(updateUser))
				.Friends.Any(x => x.Id == friendId);
		}

		public async Task<bool> RemoveFriend(Guid userId, Guid friendId)
		{
			UserServiceModel user = await this._userService.GetUserById(userId);
			UserServiceModel friendUser = await this._userService.GetUserById(friendId);

			UpdateUserServiceModel updateUser = this._userMapper.Map<UpdateUserServiceModel>(user);
			UpdateFriendServiceModel updatefriendUser = this._userMapper.Map<UpdateFriendServiceModel>(friendUser);

			updateUser.Friends.Remove(updatefriendUser);

			return !(await this._userService.UpdateUser(updateUser))
				.Friends.Any(x => x.Id == friendId);
		}
	}
}
