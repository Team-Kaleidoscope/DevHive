using Microsoft.AspNetCore.Http;

namespace DevHive.Web.Models.Identity.User
{
	public class UpdateProfilePictureWebModel
	{
		public IFormFile Picture { get; set; }
	}
}
