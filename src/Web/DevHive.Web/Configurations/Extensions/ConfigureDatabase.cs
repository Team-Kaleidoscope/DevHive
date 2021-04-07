using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DevHive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DevHive.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevHive.Web.Configurations.Extensions
{
	public static class DatabaseExtensions
	{
		public static void DatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DevHiveContext>(options =>
			{
				// options.EnableSensitiveDataLogging(true);
				options.UseNpgsql(configuration.GetConnectionString("DEV"), options =>
					{
						options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
					});
			});

			services.AddIdentity<User, Role>()
				.AddRoles<Role>()
				.AddEntityFrameworkStores<DevHiveContext>();

			services.Configure<IdentityOptions>(options =>
			{
				options.User.RequireUniqueEmail = true;

				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 5;
				options.Password.RequiredUniqueChars = 0;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;

				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("User", options =>
				{
					options.RequireAuthenticatedUser();
					options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					options.RequireRole("User");
				});

				options.AddPolicy("Administrator", options =>
				{
					options.RequireAuthenticatedUser();
					options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					options.RequireRole("Admin");
				});
			});
		}

		public static void UseDatabaseConfiguration(this IApplicationBuilder app)
		{
			app.UseHttpsRedirection();
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			using var serviceScope = app.ApplicationServices.CreateScope();
			using var dbContext = serviceScope.ServiceProvider.GetRequiredService<DevHiveContext>();

			dbContext.Database.Migrate();

			var roleManager = (RoleManager<Role>)serviceScope.ServiceProvider.GetService(typeof(RoleManager<Role>));

			if (!dbContext.Roles.Any(x => x.Name == Role.DefaultRole))
			{
				Role defaultRole = new() { Name = Role.DefaultRole };

				roleManager.CreateAsync(defaultRole).Wait();
			}

			if (!dbContext.Roles.Any(x => x.Name == Role.AdminRole))
			{
				Role adminRole = new() { Name = Role.AdminRole };

				roleManager.CreateAsync(adminRole).Wait();
			}

			dbContext.SaveChanges();
		}
	}
}
