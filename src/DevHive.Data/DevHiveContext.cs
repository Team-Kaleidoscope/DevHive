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
		public DbSet<UserFriends> UserFriends { get; set; }
		public DbSet<Rating> Rating { get; set; }
		public DbSet<RatedPost> RatedPost { get; set; }
		public DbSet<UserRate> UserRate { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			/* User */
			builder.Entity<User>()
				.HasIndex(x => x.UserName)
				.IsUnique();

			/* Roles */
			builder.Entity<User>()
				.HasMany(x => x.Roles)
				.WithMany(x => x.Users);

			/* Friends */
			builder.Entity<UserFriends>()
				.HasKey(x => new { x.UserId, x.FriendId });

			// builder.Entity<UserFriends>()
			// 	.HasOne(x => x.Friend)
			// 	.WithMany(x => x.Friends);

			builder.Entity<User>()
				.HasMany(x => x.Friends)
				.WithOne(x => x.User);

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

			// builder.Entity<Rating>()
			// 	.HasOne(x => x.Post)
			// 	.WithOne(x => x.Rating)
			// 	.HasForeignKey<Post>(x => x.RatingId);

			// builder.Entity<Post>()
			// 	.HasOne(x => x.Rating)
			// 	.WithOne(x => x.Post);

			// builder.Entity<Rating>()
			// 	.HasMany(x => x.UsersThatRated);

			// /* User Rated Posts */
			builder.Entity<RatedPost>()
				.HasKey(x => new { x.UserId, x.PostId });

			// builder.Entity<RatedPost>()
			// 	.HasOne(x => x.User)
			// 	.WithMany(x => x.RatedPosts);

			// builder.Entity<RatedPost>()
			// 	.HasOne(x => x.Post);

			// builder.Entity<User>()
			// 	.HasMany(x => x.RatedPosts);

			base.OnModelCreating(builder);
		}
	}
}
