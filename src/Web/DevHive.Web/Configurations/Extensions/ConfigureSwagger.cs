using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

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
			services.AddOpenApiDocument(c =>
			{
				c.GenerateXmlObjects = true;
				c.UseControllerSummaryAsTagDescription = true;

				c.AllowNullableBodyParameters = false;
				c.Description = "DevHive Social Media's API Endpoints";

				c.PostProcess = doc =>
				{
					doc.Info.Version = "v0.1";
					doc.Info.Title = "API";
					doc.Info.Description = "DevHive Social Media's first official API release";
					doc.Info.TermsOfService = TermsOfServiceUri;
					doc.Info.License = new NSwag.OpenApiLicense
					{
						Name = LicenseName,
						Url = LicenseUri
					};
				};

				c.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
				{
					Type = OpenApiSecuritySchemeType.ApiKey,
					Name = "Authorization",
					In = OpenApiSecurityApiKeyLocation.Header,
					Description = "Type into the textbox: Bearer {your JWT token}."
				});
				c.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
			});
		}

		public static void UseSwaggerConfiguration(this IApplicationBuilder app)
		{
			app.UseOpenApi(c =>
			{
				c.DocumentName = "v0.1";
			});
			app.UseSwaggerUi3(c =>
			{
				c.DocumentTitle = "DevHive API";
				c.EnableTryItOut = false;
				c.DocExpansion = "list";
			});
		}
	}
}
