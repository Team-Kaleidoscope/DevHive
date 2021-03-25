using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class ProfilePictureRepository : BaseRepository<ProfilePicture>, IProfilePictureRepository
	{
		private readonly DevHiveContext _context;

		public ProfilePictureRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		public async Task<ProfilePicture> GetByURLAsync(string picUrl)
		{
			return await this._context
				.ProfilePicture
				.FirstOrDefaultAsync(x => x.PictureURL == picUrl);
		}
	}
}
