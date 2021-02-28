using System;
using System.Threading.Tasks;
using DevHive.Services.Models.User;
using DevHive.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProfilePictureController
	{
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
