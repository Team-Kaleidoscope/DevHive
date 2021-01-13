using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class LanguageRepository : ILanguageRepository
	{
		private readonly DevHiveContext _context;

		public LanguageRepository(DevHiveContext context)
		{
			this._context = context;
		}

		#region Create

		public async Task<bool> AddAsync(Language entity)
		{
			await this._context
				.Set<Language>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read

		public async Task<Language> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Language>()
				.FindAsync(id);
		}
		#endregion

		#region Update

		public async Task<bool> EditAsync(Language newEntity)
		{
			this._context
			.Set<Language>()
			.Update(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete

		public async Task<bool> DeleteAsync(Language entity)
		{
			this._context
				.Set<Language>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
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