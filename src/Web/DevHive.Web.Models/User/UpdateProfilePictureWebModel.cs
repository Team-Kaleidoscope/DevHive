using Microsoft.AspNetCore.Http;

namespace DevHive.Web.Models.User
{
	public class UpdateProfilePictureWebModel
	{
		public IFormFile Picture { get; set; }
	}
}
