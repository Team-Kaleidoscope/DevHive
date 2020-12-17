using System.Collections.Generic;
using DevHive.Common.Models.Identity;

namespace DevHive.Services.Models.Identity.User
{
	public class UserServiceModel : BaseUserServiceModel
	{
		public IList<RoleModel> Roles { get; set; } = new List<RoleModel>();
		public List<UserServiceModel> Friends { get; set; } = new List<UserServiceModel>();
	}
}
