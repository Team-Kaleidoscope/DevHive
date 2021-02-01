using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Common.Models.Misc;
using DevHive.Web.Models.Identity.Role;
using DevHive.Web.Models.Language;
using DevHive.Web.Models.Technology;

namespace DevHive.Web.Models.Identity.User
{
	public class UserWebModel : BaseUserWebModel
	{
		[NotNull]
		[Required]
		public HashSet<RoleWebModel> Roles { get; set; } = new();

		[NotNull]
		[Required]
		public HashSet<UsernameWebModel> Friends { get; set; } = new();

		[NotNull]
		[Required]
		public HashSet<LanguageWebModel> Languages { get; set; } = new();

		[NotNull]
		[Required]
		public HashSet<TechnologyWebModel> Technologies { get; set; } = new();

		public List<IdModel> Posts { get; set; } = new();
	}
}
