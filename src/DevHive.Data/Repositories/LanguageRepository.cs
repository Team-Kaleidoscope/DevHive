using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class LanguageRepository : BaseRepository, ILanguageRepository
	{
		private readonly DevHiveContext _context;

		public LanguageRepository(DevHiveContext context)
		{
			this._context = context;
		}

		#region Create

		public async Task<bool> AddAsync(Language entity)
		{
			await this._context.Languages
				.AddAsync(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read

		public async Task<Language> GetByIdAsync(Guid id)
		{
			return await this._context.Languages
				.FindAsync(id);
		}

		public async Task<Language> GetByNameAsync(string languageName)
		{
			return await this._context.Languages
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Name == languageName);
		}
		#endregion

		#region Update

		public async Task<bool> EditAsync(Language entity)
		{
			Language language = await this._context.Languages
				.FirstOrDefaultAsync(x => x.Id == entity.Id);

			this._context.Update(language);
			this._context.Entry(entity).CurrentValues.SetValues(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete

		public async Task<bool> DeleteAsync(Language entity)
		{
			this._context
				.Set<Language>()
				.Remove(entity);

			return await this.SaveChangesAsync(this._context);
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
