using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DevHive.Web.Configurations.Extensions
{
	public static class SwaggerExtensions
	{
#pragma warning disable S1075
		private const string LicenseName = "GPL-3.0 License";
		private const string LicenseUri = "https://github.com/Team-Kaleidoscope/DevHive/blob/main/LICENSE";
		private const string TermsOfServiceUri = "https://example.com/terms";
#pragma warning restore S1075

		public static void SwaggerConfiguration(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v0.1", new OpenApiInfo
				{
					Version = "v0.1",
					Title = "API",
					Description = "DevHive Social Media's first official API release",
					TermsOfService = new Uri(TermsOfServiceUri),
					License = new OpenApiLicense
					{
						Name = LicenseName,
						Url = new Uri(LicenseUri)
					}
				});

				string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}

		public static void UseSwaggerConfiguration(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v0.1/swagger.json", "v0.1");
			});
		}
	}
}
