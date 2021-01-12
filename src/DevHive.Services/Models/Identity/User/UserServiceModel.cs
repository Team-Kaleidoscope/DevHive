using System.Collections.Generic;
using DevHive.Common.Models.Identity;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Models.Identity.User
{
	public class UserServiceModel : BaseUserServiceModel
	{
		public IList<RoleModel> Roles { get; set; } = new List<RoleModel>();
		public IList<UserServiceModel> Friends { get; set; } = new List<UserServiceModel>();
		public IList<LanguageServiceModel> Languages { get; set; } = new List<LanguageServiceModel>();
		public IList<TechnologyServiceModel> TechnologyServiceModels { get; set; } = new List<TechnologyServiceModel>();
	}
}
