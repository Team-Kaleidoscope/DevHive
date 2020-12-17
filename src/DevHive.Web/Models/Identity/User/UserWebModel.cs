using System.Collections.Generic;
using DevHive.Web.Models.Identity.Role;

namespace DevHive.Web.Models.Identity.User 
{
	public class UserWebModel : BaseUserWebModel
	{
		public IList<RoleWebModel> Role { get; set; } = new List<RoleWebModel>();
		public IList<UserWebModel> Friends { get; set; } = new List<UserWebModel>();
	}
}
