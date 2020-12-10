using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace API
{
	public class Program
	{
		private const int HTTPS_PORT = 5000;

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureKestrel(opt => opt.ListenLocalhost(HTTPS_PORT));

					webBuilder.UseStartup<Startup>();
				});
	}
}
