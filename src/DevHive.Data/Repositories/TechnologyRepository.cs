using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Misc;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace DevHive.Data.Repositories
{
	public abstract class TechnologyRepository : IRepository<Technology>
	{
		private readonly DevHiveContext _context;

		public TechnologyRepository(DevHiveContext context)
		{
			this._context = context;
		}

		#region Create


		public async Task<bool> AddAsync(Technology entity)
		{
			await this._context
				.Set<Technology>()
				.AddAsync(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read

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
		#endregion

		#region Edit

		public async Task<bool> EditAsync(Technology newEntity)
		{
				this._context
				.Set<Technology>()
				.Update(newEntity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete

		public async Task<bool> DeleteAsync(Technology entity)
		{
			this._context
				.Set<Technology>()
				.Remove(entity);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
		#endregion
	}
}