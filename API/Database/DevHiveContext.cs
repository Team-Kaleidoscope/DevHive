using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Models.Classes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Database
{
	public class DevHiveContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
	{
		public DevHiveContext(DbContextOptions options)
			: base(options) { }


		public DbSet<Technology> Technologies { get; set; }
		public DbSet<Language> Languages { get; set; }
	}
}