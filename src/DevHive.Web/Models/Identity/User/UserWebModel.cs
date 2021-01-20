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
		public HashSet<RoleWebModel> Roles { get; set; } = new HashSet<RoleWebModel>();

		[NotNull]
		[Required]
		public HashSet<FriendWebModel> Friends { get; set; } = new HashSet<FriendWebModel>();

		[NotNull]
		[Required]
		public HashSet<LanguageWebModel> Languages { get; set; } = new HashSet<LanguageWebModel>();

		[NotNull]
		[Required]
		public HashSet<TechnologyWebModel> Technologies { get; set; } = new HashSet<TechnologyWebModel>();
	}
}
