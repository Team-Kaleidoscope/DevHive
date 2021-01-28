using System;
using DevHive.Data.Models;
using DevHive.Data.RelationModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data
{
	public class DevHiveContext : IdentityDbContext<User, Role, Guid>
	{
		public DevHiveContext(DbContextOptions<DevHiveContext> options)
			: base(options) { }

		public DbSet<Technology> Technologies { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			/* User */
			builder.Entity<User>()
				.HasKey(x => x.Id);

			builder.Entity<User>()
				.HasIndex(x => x.UserName)
				.IsUnique();

			/* Roles */
			builder.Entity<User>()
				.HasMany(x => x.Roles)
				.WithMany(x => x.Users);

			/* Friends */
			builder.Entity<UserFriends>()
				.HasKey(uu => new { uu.UserId, uu.FriendId });

			/* Languages */
			builder.Entity<Language>()
				.HasKey(x => x.Id);

			builder.Entity<User>()
				.HasMany(x => x.Languages)
				.WithMany(x => x.Users)
				.UsingEntity(x => x.ToTable("LanguageUser"));

			builder.Entity<Language>()
				.HasMany(x => x.Users)
				.WithMany(x => x.Languages)
				.UsingEntity(x => x.ToTable("LanguageUser"));

			/* Technologies */
			builder.Entity<Technology>()
				.HasKey(x => x.Id);

			builder.Entity<User>()
				.HasMany(x => x.Technologies)
				.WithMany(x => x.Users)
				.UsingEntity(x => x.ToTable("TechnologyUser"));

			builder.Entity<Technology>()
				.HasMany(x => x.Users)
				.WithMany(x => x.Technologies)
				.UsingEntity(x => x.ToTable("TechnologyUser"));

			base.OnModelCreating(builder);
		}
	}
}
