using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DevHive.Data
{
	public class DevHiveContextFactory : IDesignTimeDbContextFactory<DevHiveContext>
	{
		public DevHiveContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("ConnectionString.json")
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseNpgsql(configuration.GetConnectionString("DEV"));

			return new DevHiveContext(optionsBuilder.Options);
		}
	}
}