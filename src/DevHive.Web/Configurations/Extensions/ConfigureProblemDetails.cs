using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureProblemDetails
	{
		public static void ProblemDetailsConfiguration(this IServiceCollection services)
		{
			services.AddProblemDetails();
		}

		public static void UseProblemDetailsConfiguration(this IApplicationBuilder app)
		{
			app.UseProblemDetails();
		}
	}
}