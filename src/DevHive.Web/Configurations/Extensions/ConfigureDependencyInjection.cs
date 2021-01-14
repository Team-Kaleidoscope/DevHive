using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Interfaces;
using DevHive.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureDependencyInjection
	{
		public static void DependencyInjectionConfiguration(this IServiceCollection services)
		{
			services.AddScoped<ILanguageRepository, LanguageRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<ITechnologyRepository, TechnologyRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IPostRepository, PostRepository>();

			services.AddScoped<ILanguageService, LanguageService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddScoped<ITechnologyService, TechnologyService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IPostService, PostService>();
		}
	}
}
