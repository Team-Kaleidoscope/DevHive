using DevHive.Web.Models.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureCustomMiddleware
	{
		public static void CustomMiddlewareConfiguration(this IServiceCollection services) { }

		public static void UseCustomMiddlewareConfiguration(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware>();
		}
	}
}
