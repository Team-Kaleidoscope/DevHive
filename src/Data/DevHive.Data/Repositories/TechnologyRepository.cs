using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class TechnologyRepository : BaseRepository<Technology>, ITechnologyRepository
	{
		private readonly DevHiveContext _context;

		public TechnologyRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		#region Read
		public async Task<Technology> GetByNameAsync(string technologyName)
		{
			return await this._context.Technologies
				.FirstOrDefaultAsync(x => x.Name == technologyName);
		}

		/// <summary>
        /// Returns all technologies that exist in the database
        /// </summary>
		public HashSet<Technology> GetTechnologies()
		{
			return this._context.Technologies.ToHashSet();
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
				.AsNoTracking()
				.AnyAsync(x => x.Id == id);
		}
		#endregion
	}
}
