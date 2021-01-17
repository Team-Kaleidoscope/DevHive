using System.Collections.Generic;
using DevHive.Web.Models.Identity.Role;
using DevHive.Web.Models.Language;
using DevHive.Web.Models.Technology;

namespace DevHive.Web.Models.Identity.User
{
	public class UserWebModel : BaseUserWebModel
	{
		public IList<RoleWebModel> Roles { get; set; } = new List<RoleWebModel>();

		public IList<UserWebModel> Friends { get; set; } = new List<UserWebModel>();

		public IList<LanguageWebModel> Languages { get; set; } = new List<LanguageWebModel>();

		public IList<TechnologyWebModel> Technologies { get; set; } = new List<TechnologyWebModel>();
	}
}
