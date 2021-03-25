using System.Text;
using DevHive.Common.Jwt;
using DevHive.Common.Jwt.Interfaces;
using DevHive.Data.Interfaces;
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
			services.AddTransient<IPostRepository, PostRepository>();
			services.AddTransient<ICommentRepository, CommentRepository>();
			services.AddTransient<IFeedRepository, FeedRepository>();
			services.AddTransient<IRatingRepository, RatingRepository>();
			services.AddTransient<IProfilePictureRepository, ProfilePictureRepository>();
			services.AddTransient<IUserRepository, UserRepository>();

			services.AddTransient<ILanguageService, LanguageService>();
			services.AddTransient<IRoleService, RoleService>();
			services.AddTransient<ITechnologyService, TechnologyService>();
			services.AddTransient<IPostService, PostService>();
			services.AddTransient<ICommentService, CommentService>();
			services.AddTransient<IFeedService, FeedService>();
			services.AddTransient<IRatingService, RatingService>();
			services.AddTransient<IProfilePictureService, ProfilePictureService>();
			services.AddTransient<IUserService, UserService>();

			services.AddTransient<ICloudService, CloudinaryService>(options =>
				new CloudinaryService(
					cloudName: configuration.GetSection("Cloud").GetSection("cloudName").Value,
					apiKey: configuration.GetSection("Cloud").GetSection("apiKey").Value,
					apiSecret: configuration.GetSection("Cloud").GetSection("apiSecret").Value));

			services.AddSingleton<IJwtService, JwtService>(options =>
				new JwtService(
					signingKey: Encoding.ASCII.GetBytes(configuration.GetSection("Jwt").GetSection("signingKey").Value),
					validationIssuer: configuration.GetSection("Jwt").GetSection("validationIssuer").Value,
					audience: configuration.GetSection("Jwt").GetSection("audience").Value));
		}
	}
}
