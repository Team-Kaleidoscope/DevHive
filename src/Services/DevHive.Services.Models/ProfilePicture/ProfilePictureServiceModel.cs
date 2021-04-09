using System;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Models.ProfilePicture
{
	public class ProfilePictureServiceModel
	{
		public Guid UserId { get; set; }
		public IFormFile ProfilePictureFormFile { get; set; }
	}
}
