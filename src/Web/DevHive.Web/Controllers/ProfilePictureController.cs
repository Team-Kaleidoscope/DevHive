using System;
using System.Threading.Tasks;
using DevHive.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the profile picture layer
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class ProfilePictureController
	{
		// private readonly ProfilePictureService _profilePictureService;

		// public ProfilePictureController(ProfilePictureService profilePictureService)
		// {
		// 	this._profilePictureService = profilePictureService;
		// }

		/// <summary>
		/// Alter the profile picture of a user
		/// </summary>
		/// <param name="userId">The user's Id</param>
		/// <param name="updateProfilePictureWebModel">The new profile picture</param>
		/// <param name="authorization">JWT Bearer Token</param>
		/// <returns>???</returns>
		[HttpPut]
		[Route("ProfilePicture")]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> UpdateProfilePicture(Guid userId, [FromForm] UpdateProfilePictureWebModel updateProfilePictureWebModel, [FromHeader] string authorization)
		{
			throw new NotImplementedException();
			// if (!await this._userService.ValidJWT(userId, authorization))
			// 	return new UnauthorizedResult();

			// UpdateProfilePictureServiceModel updateProfilePictureServiceModel = this._userMapper.Map<UpdateProfilePictureServiceModel>(updateProfilePictureWebModel);
			// updateProfilePictureServiceModel.UserId = userId;

			// ProfilePictureServiceModel profilePictureServiceModel = await this._userService.UpdateProfilePicture(updateProfilePictureServiceModel);
			// ProfilePictureWebModel profilePictureWebModel = this._userMapper.Map<ProfilePictureWebModel>(profilePictureServiceModel);

			// return new AcceptedResult("UpdateProfilePicture", profilePictureWebModel);
		}
	}
}
