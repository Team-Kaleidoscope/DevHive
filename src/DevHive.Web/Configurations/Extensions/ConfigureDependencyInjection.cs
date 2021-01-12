using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureDependencyInjection
	{
		public static void DependencyInjectionConfiguration(this IServiceCollection services)
		{
			services.AddTransient<LanguageRepository>();
			services.AddTransient<RoleRepository>();
			services.AddTransient<TechnologyRepository>();
			services.AddTransient<UserRepository>();
			services.AddTransient<PostRepository>();

			services.AddTransient<LanguageService>();
			services.AddTransient<RoleService>();
			services.AddTransient<TechnologyService>();
			services.AddTransient<UserService>();
			services.AddTransient<PostService>();
		}
	}
}