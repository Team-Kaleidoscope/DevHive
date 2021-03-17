using System;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces
{
	public interface IRoleRepository : IRepository<Role>
	{
		Task<Role> GetByNameAsync(string name);

		Task<bool> DoesNameExist(string name);
		Task<bool> DoesRoleExist(Guid id);
	}
}
