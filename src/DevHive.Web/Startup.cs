using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevHive.Web.Configurations.Extensions;
using AutoMapper;

namespace DevHive.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.DatabaseConfiguration(Configuration);
			services.SwaggerConfiguration();
			services.JWTConfiguration(Configuration);
			services.AutoMapperConfiguration();			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				//app.UseDeveloperExceptionPage();
				app.UseExceptionHandler("/api/Error");
				//app.UseExceptionHandler("/api/HttpError");
				app.UseSwaggerConfiguration();
			}
			else
			{
				app.UseExceptionHandler("/api/HttpError");
				app.UseHsts();
			}

			app.UseDatabaseConfiguration();

			
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "api/{controller}/{action}"
				);
			});
		}
	}
}
