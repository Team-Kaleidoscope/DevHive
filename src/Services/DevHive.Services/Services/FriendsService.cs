using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Services.Interfaces;

namespace DevHive.Services.Services
{
	public class FriendsService : IFriendsService
	{
		private readonly IUserService _userService;
		private readonly IMapper _userMapper;
		private readonly IJwtService _jwtService;

		public FriendsService(IUserService userService,
			IMapper mapper,
			IJwtService jwtService)
		{
			this._userService = userService;
			this._userMapper = mapper;
			this._jwtService = jwtService;
		}

		public async Task<object> AddFriend(Guid userId, Guid friendId)
		{
			return new { Message = "FUCK YOU" };
		}

		public async Task<object> RemoveFriend(Guid userId, Guid friendId)
		{
			return new { Message = "FUCK YOU" };
		}
	}
}
