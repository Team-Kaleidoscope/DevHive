using DevHive.Data.Models;
using System;
using System.Threading.Tasks;

namespace DevHive.Data.Repositories.Contracts
{
    public interface ITechnologyRepository : IRepository<Technology>
    {
		public Task<bool> DoesTechnologyNameExist(string technologyName);

		public Task<bool> DoesTechnologyExist(Guid id);
	}
}
