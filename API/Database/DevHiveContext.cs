using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Data.Models.Classes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace API.Database
{
	public class DevHiveContext : IdentityDbContext<User, Roles, int>
	{
		public DevHiveContext(DbContextOptions options)
			: base(options) { }

		public DbSet<Technology> Technologies { get; set; }
		public DbSet<Language> Languages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<User>()
				.HasKey(x => x.Id);

			builder.Entity<User>()
				.HasIndex(x => x.UserName)
				.IsUnique();

			builder.Entity<Language>()
				.HasKey(x => x.Id);

			builder.Entity<Technology>()
				.HasKey(x => x.Id);

			base.OnModelCreating(builder);
		}
	}
}
