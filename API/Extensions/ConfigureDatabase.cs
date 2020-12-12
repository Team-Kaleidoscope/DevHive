using Microsoft.Extensions.DependencyInjection;
using API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Models.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions
{
	public static class DatabaseExtensions
	{
		public static void DatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DevHiveContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("DEV")))
				.AddAuthentication()
				.AddJwtBearer();

			services.AddIdentity<User, Roles>()
				.AddEntityFrameworkStores<DevHiveContext>();
				
			services.AddAuthentication();

			services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;

				options.Password.RequiredLength = 5;
			});
		}

		public static void UseDatabaseConfiguration(this IApplicationBuilder app)
		{
			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
		}
	}
}