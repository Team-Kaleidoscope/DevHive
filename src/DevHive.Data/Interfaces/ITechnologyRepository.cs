using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface ITechnologyRepository : IRepository<Technology>
	{
		Task<bool> DoesTechnologyExistAsync(Guid id);
		Task<bool> DoesTechnologyNameExistAsync(string technologyName);
		Task<Technology> GetByNameAsync(string name);
	}
}
