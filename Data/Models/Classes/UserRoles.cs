using Microsoft.AspNetCore.Identity;

namespace Data.Models.Classes
{
	public class UserRoles : IdentityRole<int>
	{
		public const string User = "User";
		public const string Admin = "Admin";
	}
}
