using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Attributes;
using DevHive.Web.Models.Role;
using DevHive.Web.Models.Language;
using DevHive.Web.Models.Technology;

namespace DevHive.Web.Models.User
{
	public class UpdateUserWebModel : BaseUserWebModel
	{
		[NotNull]
		[Required]
		[GoodPassword]
		public string Password { get; set; }

		[NotNull]
		[Required]
		public HashSet<UsernameWebModel> Friends { get; set; }

		[NotNull]
		[Required]
		public HashSet<UpdateRoleWebModel> Roles { get; set; }

		[NotNull]
		[Required]
		public HashSet<UpdateLanguageWebModel> Languages { get; set; }

		[NotNull]
		[Required]
		public HashSet<UpdateTechnologyWebModel> Technologies { get; set; }
	}
}
