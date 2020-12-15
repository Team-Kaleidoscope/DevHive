using System;
using System.Collections.Generic;
using System.Linq;
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
		public async Task AddAsync(Language entity)
		{
			await this._context
				.Set<Language>()
				.AddAsync(entity);

			await this._context.SaveChangesAsync();
		}

		//Read
		public async Task<Language> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Language>()
				.FindAsync(id);
		}

		//Update
		public async Task EditAsync(Language newEntity)
		{
				this._context
				.Set<Language>()
				.Update(newEntity);

			await this._context.SaveChangesAsync();
		}

		//Delete
		public async Task DeleteAsync(Language entity)
		{
			this._context
				.Set<Language>()
				.Remove(entity);

			await this._context.SaveChangesAsync();
		}
	}
}