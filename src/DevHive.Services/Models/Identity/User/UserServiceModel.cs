using System.Collections.Generic;
using DevHive.Common.Models.Identity;
using DevHive.Data.Models;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Services.Models.Identity.User
{
	public class UserServiceModel : BaseUserServiceModel
	{
		public IList<RoleModel> Role { get; set; } = new List<RoleModel>();
		public List<UserServiceModel> Friends { get; set; } = new List<UserServiceModel>();
	}
}
