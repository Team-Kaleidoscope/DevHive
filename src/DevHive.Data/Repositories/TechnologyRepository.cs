using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Misc;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace DevHive.Data.Repositories
{
	public class TechnologyRepository : BaseRepository, ITechnologyRepository
	{
		private readonly DevHiveContext _context;

		public TechnologyRepository(DevHiveContext context)
		{
			this._context = context;
		}

		#region Create
		public async Task<bool> AddAsync(Technology entity)
		{
			await this._context.Technologies
				.AddAsync(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Read
		public async Task<Technology> GetByIdAsync(Guid id)
		{
			return await this._context.Technologies
				.FindAsync(id);
		}
		public async Task<Technology> GetByNameAsync(string technologyName)
		{
			return await this._context.Technologies
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Name == technologyName);
		}
		#endregion

		#region Edit
		public async Task<bool> EditAsync(Technology newEntity)
		{
			this._context.Technologies.Update(newEntity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteAsync(Technology entity)
		{
			this._context.Technologies
				.Remove(entity);

			return await this.SaveChangesAsync(this._context);
		}
		#endregion

		#region Validations
		public async Task<bool> DoesTechnologyNameExistAsync(string technologyName)
		{
			return await this._context.Technologies
				.AsNoTracking()
				.AnyAsync(r => r.Name == technologyName);
		}

		public async Task<bool> DoesTechnologyExistAsync(Guid id)
		{
			return await this._context.Technologies
				.AnyAsync(x => x.Id == id);
		}
		#endregion
	}
}
