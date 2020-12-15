using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Data.Models;

namespace DevHive.Data.Repositories
{
	public class RoleRepository : IRepository<Role>
	{
		public async Task AddAsync(Role entity)
		{
			throw new NotImplementedException();
		}

		//Find entity by id
		public async Task<TEntity> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		//Modify Entity from database
		public async Task EditAsync(Role newEntity)
		{
			throw new NotImplementedException();
		}

		//Delete Entity from database
		public async Task DeleteAsync(Role entity)
		{
			throw new NotImplementedException();
		}
	}
}
