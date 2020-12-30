using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Common.Models.Misc;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class LanguageRepository : IRepository<Language>
	{
		private readonly DevHiveContext _context;

		public LanguageRepository(DevHiveContext context)
		{
			this._context = context;
		}

		//Create
		public async Task<bool> AddAsync(Language entity)
		{
			await this._context
				.Set<Language>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Read
		public async Task<Language> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Language>()
				.FindAsync(id);
		}

		public async Task<bool> DoesLanguageNameExist(string languageName)
		{
			return await this._context
				.Set<Language>()
				.AsNoTracking()
				.AnyAsync(r => r.Name == languageName);
		}

		public async Task<bool> DoesLanguageExist(Guid id)
		{
			return await this._context
				.Set<Language>()
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}

		//Update
		public async Task<bool> EditAsync(Language newEntity)
		{
				this._context
				.Set<Language>()
				.Update(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Delete
		public async Task<bool> DeleteAsync(Language entity)
		{
			this._context
				.Set<Language>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
	}
}