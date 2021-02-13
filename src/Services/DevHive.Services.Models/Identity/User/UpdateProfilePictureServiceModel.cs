using System;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Models.Identity.User
{
	public class UpdateProfilePictureServiceModel
	{
		public Guid UserId { get; set; }

		public IFormFile Picture { get; set; }
	}
}
