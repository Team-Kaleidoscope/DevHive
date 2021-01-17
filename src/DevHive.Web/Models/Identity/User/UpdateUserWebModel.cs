using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevHive.Web.Models.Identity.Validation;
using DevHive.Web.Models.Language;
using DevHive.Web.Models.Technology;

namespace DevHive.Web.Models.Identity.User
{
	public class UpdateUserWebModel : BaseUserWebModel
	{
		[Required]
		[GoodPassword]
		public string Password { get; set; }

		public IList<FriendWebModel> Friends { get; set; }

		public IList<UpdateLanguageWebModel> Languages { get; set; }

		public IList<UpdateTechnologyWebModel> Technologies { get; set; }
	}
}
