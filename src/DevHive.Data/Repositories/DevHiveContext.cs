using System;
using DevHive.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class DevHiveContext : IdentityDbContext<User, Role<Guid>, Guid>
	{
		public DevHiveContext(DbContextOptions options)
			: base(options) { }

		public DbSet<Technology> Technologies { get; set; }
		public DbSet<Language> Languages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<User>()
				.HasIndex(x => x.UserName)
				.IsUnique();

			base.OnModelCreating(builder);
		}
	}
}
