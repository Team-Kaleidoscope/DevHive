using System.Text;
using System.Threading.Tasks;
using DevHive.Services.Options;
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
			services.AddSingleton(new JwtOptions(configuration
						.GetSection("AppSettings")
						.GetSection("Secret")
						.Value));

			// Get key from appsettings.json
			var key = Encoding.ASCII.GetBytes(configuration
						.GetSection("AppSettings")
						.GetSection("Secret")
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
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}
	}
}
