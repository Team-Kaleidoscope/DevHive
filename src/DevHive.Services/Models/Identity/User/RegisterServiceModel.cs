using System.Collections.Generic;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Models.Identity.User
{
	public class RegisterServiceModel : BaseUserServiceModel
	{
		public string Password { get; set; }

		public HashSet<LanguageServiceModel> Languages { get; set; }

		public HashSet<TechnologyServiceModel> Technologies { get; set; }
	}
}
