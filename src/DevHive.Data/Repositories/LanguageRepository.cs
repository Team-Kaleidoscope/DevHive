using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class LanguageRepository : BaseRepository<Language>, ILanguageRepository
	{
		private readonly DevHiveContext _context;

		public LanguageRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		#region Read
		public async Task<Language> GetByNameAsync(string languageName)
		{
			return await this._context.Languages
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Name == languageName);
		}
		#endregion

		#region Validations
		public async Task<bool> DoesLanguageNameExistAsync(string languageName)
		{
			return await this._context.Languages
				.AnyAsync(r => r.Name == languageName);
		}

		public async Task<bool> DoesLanguageExistAsync(Guid id)
		{
			return await this._context.Languages
				.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
