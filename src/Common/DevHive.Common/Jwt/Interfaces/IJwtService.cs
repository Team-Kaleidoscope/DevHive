using System;
using System.Collections.Generic;

namespace DevHive.Common.Jwt.Interfaces
{
	public interface IJwtService
	{
		string GenerateJwtToken(Guid userId, string username, List<string> roleNames);
		bool ValidateToken(Guid userId, string rawToken);
	}
}
