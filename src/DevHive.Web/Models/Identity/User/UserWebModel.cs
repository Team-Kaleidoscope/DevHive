using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Models.Identity.Role;
using DevHive.Web.Models.Language;
using DevHive.Web.Models.Technology;

namespace DevHive.Web.Models.Identity.User
{
	public class UserWebModel : BaseUserWebModel
	{
		[NotNull]
		[Required]
		public IList<RoleWebModel> Roles { get; set; } = new List<RoleWebModel>();

		[NotNull]
		[Required]
		public IList<UserWebModel> Friends { get; set; } = new List<UserWebModel>();

		[NotNull]
		[Required]
		public IList<LanguageWebModel> Languages { get; set; } = new List<LanguageWebModel>();

		[NotNull]
		[Required]
		public IList<TechnologyWebModel> Technologies { get; set; } = new List<TechnologyWebModel>();
	}
}
