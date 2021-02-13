using System;
using System.Collections.Generic;
using System.Linq;
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
				.FirstOrDefaultAsync(x => x.Name == languageName);
		}

		/// <summary>
        /// Returns all technologies that exist in the database
        /// </summary>
		public HashSet<Language> GetLanguages()
		{
			return this._context.Languages.ToHashSet();
		}
		#endregion

		#region Validations
		public async Task<bool> DoesLanguageNameExistAsync(string languageName)
		{
			return await this._context.Languages
				.AsNoTracking()
				.AnyAsync(r => r.Name == languageName);
		}

		public async Task<bool> DoesLanguageExistAsync(Guid id)
		{
			return await this._context.Languages
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
