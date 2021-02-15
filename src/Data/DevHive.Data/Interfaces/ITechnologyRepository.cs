using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface ITechnologyRepository : IRepository<Technology>
	{
		Task<Technology> GetByNameAsync(string technologyName);
		HashSet<Technology> GetTechnologies();

		Task<bool> DoesTechnologyExistAsync(Guid id);
		Task<bool> DoesTechnologyNameExistAsync(string technologyName);
	}
}
