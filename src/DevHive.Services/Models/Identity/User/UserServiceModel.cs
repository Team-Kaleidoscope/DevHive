using System.Collections.Generic;
using DevHive.Data.Models;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Services.Models.Identity.User
{
	public class UserServiceModel : BaseUserServiceModel
	{
		public IList<RoleServiceModel> Role { get; set; } = new List<RoleServiceModel>();
		public List<UserServiceModel> Friends { get; set; } = new List<UserServiceModel>();
	}
}
