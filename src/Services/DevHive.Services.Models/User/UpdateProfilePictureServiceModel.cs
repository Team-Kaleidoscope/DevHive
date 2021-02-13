using System;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Models.User
{
	public class UpdateProfilePictureServiceModel
	{
		public Guid UserId { get; set; }

		public IFormFile Picture { get; set; }
	}
}
