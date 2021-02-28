using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using DevHive.Common.Jwt.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DevHive.Common.Jwt
{
	public class JwtService : IJwtService
	{
		private readonly string _validationIssuer;
		private readonly string _audience;
		private readonly byte[] _signingKey;

		public JwtService(byte[] signingKey, string validationIssuer, string audience)
		{
			this._signingKey = signingKey;
			this._validationIssuer = validationIssuer;
			this._audience = audience;
		}

		public string GenerateJwtToken(Guid userId, string username, List<string> roleNames)
		{
			var securityKey = new SymmetricSecurityKey(this._signingKey);
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			HashSet<Claim> claims = new()
			{
				new Claim("ID", $"{userId}"),
				new Claim("Username", username)
			};

			foreach (var roleName in roleNames)
				claims.Add(new Claim(ClaimTypes.Role, roleName));

			SecurityTokenDescriptor securityTokenDescriptor = new()
			{
				Issuer = this._validationIssuer,
				Audience = this._audience,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Today.AddDays(7),
				SigningCredentials = credentials,
			};

			JwtSecurityTokenHandler tokenHandler = new();
			SecurityToken token = tokenHandler.CreateToken(securityTokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		public bool ValidateToken(Guid userId, string rawToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = GetValidationParameters();
			string actualToken = rawToken.Remove(0, 7);

			IPrincipal principal = tokenHandler.ValidateToken(actualToken, validationParameters, out SecurityToken validatedToken);
			JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(actualToken);

			if (!principal.Identity.IsAuthenticated)
				return false;
			else if (principal.IsInRole("Admin"))
				return true;
			else if (jwtToken.Claims.FirstOrDefault(x => x.Type == "ID").Value != userId.ToString())
				return false;
			else
				return true;
		}

		private TokenValidationParameters GetValidationParameters()
		{
			return new TokenValidationParameters()
			{
				ValidateLifetime = true,
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidIssuer = this._validationIssuer,
				ValidAudience = this._audience,
				IssuerSigningKey = new SymmetricSecurityKey(this._signingKey)
			};
		}
	}
}
