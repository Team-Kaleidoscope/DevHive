using Microsoft.Extensions.DependencyInjection;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DevHive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;

namespace DevHive.Web.Configurations.Extensions
{
	public static class DatabaseExtensions
	{
		public static void DatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DevHiveContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("DEV")));

			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<DevHiveContext>();
				
			services.Configure<IdentityOptions>(options =>
			{
				//TODO: Add more validations
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
