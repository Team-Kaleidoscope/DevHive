using System.Collections.Generic;
using DevHive.Common.Models.Misc;
using DevHive.Services.Models.Role;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Models.User
{
	public class UserServiceModel : BaseUserServiceModel
	{
		public string ProfilePictureURL { get; set; }

		public HashSet<RoleServiceModel> Roles { get; set; } = new();

		public HashSet<FriendServiceModel> Friends { get; set; } = new();

		public HashSet<LanguageServiceModel> Languages { get; set; } = new();

		public HashSet<TechnologyServiceModel> Technologies { get; set; } = new();

		public List<IdModel> Posts { get; set; } = new();
	}
}
