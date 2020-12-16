using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Common.Models.Data
{
	public static class RepositoryMethods
	{
		public static async Task<bool> SaveChangesAsync(DbContext context)
		{
			int result = await context.SaveChangesAsync();

			return result >= 1;
		}
	}
}