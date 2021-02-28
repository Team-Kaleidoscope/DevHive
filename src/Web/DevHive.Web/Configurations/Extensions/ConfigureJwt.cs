using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureJwt
	{
		public static void JWTConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			// Get key from appsettings.json
			var signingKey = Encoding.ASCII.GetBytes(configuration
						.GetSection("Jwt")
						.GetSection("signingKey")
						.Value);

			// Setup Jwt Authentication
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.Events = new JwtBearerEvents
				{
					OnTokenValidated = context =>
					{
						return Task.CompletedTask;
					}
				};
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(signingKey),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}
	}
}
