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
			services.AddTransient<ILanguageRepository, LanguageRepository>();
			services.AddTransient<IRoleRepository, RoleRepository>();
			services.AddTransient<ITechnologyRepository, TechnologyRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IPostRepository, PostRepository>();

			services.AddTransient<ILanguageService, LanguageService>();
			services.AddTransient<IRoleService, RoleService>();
			services.AddTransient<ITechnologyService, TechnologyService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IPostService, PostService>();
		}
	}
}
