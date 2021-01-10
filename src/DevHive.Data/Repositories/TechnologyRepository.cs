using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Common.Models.Misc;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace DevHive.Data.Repositories
{
	public class TechnologyRepository : IRepository<Technology>
	{
		private readonly DevHiveContext _context;

		public TechnologyRepository(DevHiveContext context)
		{
			this._context = context;
		}

		//Create
		public async Task<bool> AddAsync(Technology entity)
		{
			await this._context
				.Set<Technology>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Read		
		public async Task<Technology> GetByIdAsync(Guid id)
		{
			return await this._context
				.Set<Technology>()
				.FindAsync(id);
		}

		public async Task<bool> DoesTechnologyNameExist(string technologyName)
		{
			return await this._context
				.Set<Technology>()
				.AsNoTracking()
				.AnyAsync(r => r.Name == technologyName);
		}

		public async Task<bool> DoesTechnologyExist(Guid id)
		{
			return await this._context
				.Set<Technology>()
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}

		//Edit
		public async Task<bool> EditAsync(Technology newEntity)
		{
				this._context
				.Set<Technology>()
				.Update(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Delete
		public async Task<bool> DeleteAsync(Technology entity)
		{
			this._context
				.Set<Technology>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
	}
}