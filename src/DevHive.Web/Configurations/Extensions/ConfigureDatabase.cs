using Microsoft.Extensions.DependencyInjection;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DevHive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DevHive.Data;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace DevHive.Web.Configurations.Extensions
{
	public static class DatabaseExtensions
	{
		public static void DatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<DevHiveContext>(options =>
			{
				options.EnableSensitiveDataLogging(true);
				options.UseNpgsql(configuration.GetConnectionString("DEV"));
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
		}
	}
}
