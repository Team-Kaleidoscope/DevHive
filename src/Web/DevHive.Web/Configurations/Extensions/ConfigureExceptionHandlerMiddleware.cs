using DevHive.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureExceptionHandlerMiddleware
	{
		public static void ExceptionHandlerMiddlewareConfiguration(this IServiceCollection services) { }

		public static void UseExceptionHandlerMiddlewareConfiguration(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware>();
		}
	}
}
