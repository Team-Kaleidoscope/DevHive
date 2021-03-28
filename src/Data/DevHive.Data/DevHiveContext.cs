using System;
using DevHive.Data.Models;
using DevHive.Data.Models.Relational;
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
		public DbSet<PostAttachments> PostAttachments { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Rating> Rating { get; set; }
		public DbSet<ProfilePicture> ProfilePicture { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			/* User */
			builder.Entity<User>()
				.HasIndex(x => x.UserName)
				.IsUnique();

			builder.Entity<User>()
				.HasOne(x => x.ProfilePicture);

			/* Roles */
			builder.Entity<User>()
				.HasMany(x => x.Roles)
				.WithMany(x => x.Users);

			/* Languages */
			builder.Entity<User>()
				.HasMany(x => x.Languages)
				.WithMany(x => x.Users)
				.UsingEntity(x => x.ToTable("LanguageUser"));

			builder.Entity<Language>()
				.HasMany(x => x.Users)
				.WithMany(x => x.Languages)
				.UsingEntity(x => x.ToTable("LanguageUser"));

			/* Technologies */
			builder.Entity<User>()
				.HasMany(x => x.Technologies)
				.WithMany(x => x.Users)
				.UsingEntity(x => x.ToTable("TechnologyUser"));

			builder.Entity<Technology>()
				.HasMany(x => x.Users)
				.WithMany(x => x.Technologies)
				.UsingEntity(x => x.ToTable("TechnologyUser"));

			/* Post */
			builder.Entity<Post>()
				.HasOne(x => x.Creator)
				.WithMany(x => x.Posts);

			builder.Entity<Post>()
				.HasMany(x => x.Comments)
				.WithOne(x => x.Post);

			/* Post attachments */
			builder.Entity<PostAttachments>()
				.HasOne(x => x.Post)
				.WithMany(x => x.Attachments);

			/* Comment */
			builder.Entity<Comment>()
				.HasOne(x => x.Post)
				.WithMany(x => x.Comments);

			builder.Entity<Comment>()
				.HasOne(x => x.Creator)
				.WithMany(x => x.Comments);

			/* Rating */
			builder.Entity<Rating>()
				.HasKey(x => x.Id);

			builder.Entity<Rating>()
			 	.HasOne(x => x.Post)
			 	.WithMany(x => x.Ratings);

			builder.Entity<Post>()
			 	.HasMany(x => x.Ratings)
			 	.WithOne(x => x.Post);

			base.OnModelCreating(builder);
		}
	}
}
