using System.Collections.Generic;
using DevHive.Services.Models.Identity.Role;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Models.Identity.User
{
    public class UserServiceModel : BaseUserServiceModel
	{
		public IList<RoleServiceModel> Roles { get; set; } = new List<RoleServiceModel>();

		public IList<FriendServiceModel> Friends { get; set; } = new List<FriendServiceModel>();

		public IList<LanguageServiceModel> Languages { get; set; } = new List<LanguageServiceModel>();

		public IList<TechnologyServiceModel> Technologies { get; set; } = new List<TechnologyServiceModel>();
	}
}
