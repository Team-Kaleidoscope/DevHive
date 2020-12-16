using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class LanguageRepository : IRepository<Language>
	{
		private readonly DbContext _context;

		public LanguageRepository(DbContext context)
		{
			this._context = context;
		}

		//Create
		public async Task<bool> AddAsync(Language entity)
		{
			await this._context
				.Set<Language>()
				.AddAsync(entity);

			return await this.SaveChangesAsync();
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

			return await this.SaveChangesAsync();
		}

		//Delete
		public async Task<bool> DeleteAsync(Language entity)
		{
			this._context
				.Set<Language>()
				.Remove(entity);

			return await this.SaveChangesAsync();
		}
	
		private async Task<bool> SaveChangesAsync()
        {
            int result = await this._context.SaveChangesAsync();

            return result >= 0;
        }
	}
}