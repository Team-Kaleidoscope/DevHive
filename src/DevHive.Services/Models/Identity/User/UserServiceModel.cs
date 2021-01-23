using System.Collections.Generic;
using DevHive.Services.Models.Identity.Role;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Models.Identity.User
{
	public class UserServiceModel : BaseUserServiceModel
	{
		public HashSet<RoleServiceModel> Roles { get; set; } = new HashSet<RoleServiceModel>();

		public HashSet<FriendServiceModel> Friends { get; set; } = new HashSet<FriendServiceModel>();

		public HashSet<LanguageServiceModel> Languages { get; set; } = new HashSet<LanguageServiceModel>();

		public HashSet<TechnologyServiceModel> Technologies { get; set; } = new HashSet<TechnologyServiceModel>();
	}
}
