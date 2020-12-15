using System;
using System.Threading.Tasks;
using Data.Models.Interfaces.Database;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RoleRepository : IRepository<Role>
	{
		private readonly DbContext _context;

		public RoleRepository(DbContext context)
		{
			this._context = context;
		}

		public async Task AddAsync(Role entity)
		{
			throw new NotImplementedException();
		}

		public async Task<Role> GetByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task EditAsync(Role newEntity)
		{
			throw new NotImplementedException();
		}

		public async Task DeleteAsync(Role entity)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DoesNameExist(string name)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DoesRoleExist(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
