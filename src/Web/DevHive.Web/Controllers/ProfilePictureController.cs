using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.ProfilePicture;
using DevHive.Common.Jwt.Interfaces;
using AutoMapper;
using DevHive.Web.Models.ProfilePicture;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the profile picture layer
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class ProfilePictureController
	{
		private readonly IProfilePictureService _profilePictureService;
		private readonly IJwtService _jwtService;
		private readonly IMapper _profilePictureMapper;

		public ProfilePictureController(IProfilePictureService profilePictureService, IJwtService jwtService, IMapper profilePictureMapper)
		{
			this._profilePictureService = profilePictureService;
			this._jwtService = jwtService;
			this._profilePictureMapper = profilePictureMapper;
		}

		/// <summary>
		/// Get the URL of user's profile picture
		/// </summary>
		/// <param name="profilePictureId">The profile picture's Id</param>
		/// <param name="authorization">JWT Bearer Token</param>
		/// <returns>The URL of the profile picture</returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> ReadProfilePicture(Guid profilePictureId)
		{
			string profilePicURL = await this._profilePictureService.GetProfilePictureById(profilePictureId);
			return new OkObjectResult(new { ProfilePictureURL = profilePicURL} );
		}

		/// <summary>
		/// Alter the profile picture of a user
		/// </summary>
		/// <param name="userId">The user's Id</param>
		/// <param name="profilePictureWebModel">The new profile picture</param>
		/// <param name="authorization">JWT Bearer Token</param>
		/// <returns>The URL of the new profile picture</returns>
		[HttpPut]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> UpdateProfilePicture(Guid userId, [FromForm] ProfilePictureWebModel profilePictureWebModel, [FromHeader] string authorization)
		{
			if (!this._jwtService.ValidateToken(userId, authorization))
				return new UnauthorizedResult();

			ProfilePictureServiceModel profilePictureServiceModel = this._profilePictureMapper.Map<ProfilePictureServiceModel>(profilePictureWebModel);
			profilePictureServiceModel.UserId = userId;

			string url = await this._profilePictureService.UpdateProfilePicture(profilePictureServiceModel);
			return new OkObjectResult(new { URL = url });
		}
	}
}
