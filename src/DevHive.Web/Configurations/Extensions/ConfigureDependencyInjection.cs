using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Interfaces;
using DevHive.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevHive.Web.Configurations.Extensions
{
	public static class ConfigureDependencyInjection
	{
		public static void DependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<ILanguageRepository, LanguageRepository>();
			services.AddTransient<IRoleRepository, RoleRepository>();
			services.AddTransient<ITechnologyRepository, TechnologyRepository>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IPostRepository, PostRepository>();
			services.AddTransient<ICommentRepository, CommentRepository>();
			services.AddTransient<IFeedRepository, FeedRepository>();

			services.AddTransient<ILanguageService, LanguageService>();
			services.AddTransient<IRoleService, RoleService>();
			services.AddTransient<ITechnologyService, TechnologyService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IPostService, PostService>();
			services.AddTransient<IFeedService, FeedService>();
			services.AddTransient<ICloudService, CloudinaryService>(options =>
				new CloudinaryService(
					cloudName: configuration.GetSection("Cloud").GetSection("cloudName").Value,
					apiKey: configuration.GetSection("Cloud").GetSection("apiKey").Value,
					apiSecret: configuration.GetSection("Cloud").GetSection("apiSecret").Value));
		}
	}
}
