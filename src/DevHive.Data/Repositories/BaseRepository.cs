using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class BaseRepository
	{
		public async Task<bool> SaveChangesAsync(DbContext context)
		{
			int result = await context.SaveChangesAsync();

			return result >= 1;
		}
	}
}
