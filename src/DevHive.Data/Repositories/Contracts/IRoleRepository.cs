using DevHive.Data.Models;
using System;
using System.Threading.Tasks;

namespace DevHive.Data.Repositories.Contracts
{
    public interface IRoleRepository : IRepository<Role>
    {
		public Task<Role> GetByNameAsync(string name);

		public Task<bool> DoesNameExist(string name);

		public Task<bool> DoesRoleExist(Guid id);
	}
}
