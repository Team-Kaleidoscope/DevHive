using Microsoft.AspNetCore.Http;

namespace DevHive.Web.Models.ProfilePicture
{
	public class ProfilePictureWebModel
	{
		public IFormFile Picture { get; set; }
	}
}
