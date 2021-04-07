using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Common.Models.Identity;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.User;
using DevHive.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FriendsController
	{
		private readonly IFriendsService _friendsService;
		private readonly IMapper _mapper;
		private readonly IJwtService _jwtService;

		public FriendsController(IFriendsService friendsService, IMapper mapper, IJwtService jwtService)
		{
			this._friendsService = friendsService;
			this._mapper = mapper;
			this._jwtService = jwtService;
		}

		[HttpPost]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> AddFriend(Guid userId, Guid friendId, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			return null;
		}

		[HttpDelete]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> RemoveFriend(Guid userId, Guid friendId, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			return null;
		}
	}
}
