using System;
using System.Collections.Generic;

namespace DevHive.Common.Jwt.Interfaces
{
	public interface IJwtService
	{
		/// <summary>
		/// The generation of a JWT, when a new user registers or log ins
		/// Tokens have an expiration time of 7 days.
		/// </summary>
		/// <param name="userId">User's Guid</param>
		/// <param name="username">Users's username</param>
		/// <param name="roleNames">List of user's roles</param>
		/// <returns>Return a new JWT, containing the user id, username and roles.</returns>
		string GenerateJwtToken(Guid userId, string username, List<string> roleNames);

		/// <summary>
		/// Checks whether the given user, gotten by the "id" property,
		/// is the same user as the one in the token (unless the user in the token has the admin role)
		/// and the roles in the token are the same as those in the user, gotten by the id in the token
		/// </summary>
		/// <param name="userId">Guid of the user being validated</param>
		/// <param name="rawToken">The raw token coming from the request</param>
		/// <returns>Bool result of is the user authenticated to do an action</returns>
		bool ValidateToken(Guid userId, string rawToken);
	}
}
