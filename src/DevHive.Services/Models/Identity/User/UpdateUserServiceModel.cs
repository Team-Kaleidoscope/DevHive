using System;
using System.Collections.Generic;
using DevHive.Services.Models.Identity.Role;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Models.Identity.User
{
	public class UpdateUserServiceModel : BaseUserServiceModel
	{
		public Guid Id { get; set; }

		public string Password { get; set; }

		public HashSet<UpdateRoleServiceModel> Roles { get; set; } = new HashSet<UpdateRoleServiceModel>();

		public HashSet<UpdateFriendServiceModel> Friends { get; set; } = new HashSet<UpdateFriendServiceModel>();

		public HashSet<UpdateLanguageServiceModel> Languages { get; set; } = new HashSet<UpdateLanguageServiceModel>();

		public HashSet<UpdateTechnologyServiceModel> Technologies { get; set; } = new HashSet<UpdateTechnologyServiceModel>();

	}
}
