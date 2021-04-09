using DevHive.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureExceptionHandlerMiddleware
	{
		public static void ConfigureExceptionHandler(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<ApiBehaviorOptions>(o =>
			{
				o.InvalidModelStateResponseFactory = actionContext =>
				{
					var problemDetails = new ValidationProblemDetails(actionContext.ModelState)
					{
						Status = StatusCodes.Status422UnprocessableEntity
					};

					return new UnprocessableEntityObjectResult(problemDetails)
					{
						ContentTypes = { "application/problem+json" }
					};
				};
			});
		}

		public static void UseExceptionHandlerMiddlewareConfiguration(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware>();
		}
	}
}
