using System;
using System.Threading.Tasks;

namespace DevHive.Data.Repositories
{
	public interface IRepository<TEntity>
		where TEntity : class
	{
		//Add Entity to database
		Task<bool> AddAsync(TEntity entity);

		//Find entity by id
		Task<TEntity> GetByIdAsync(Guid id);

		//Modify Entity from database
		Task<bool> EditAsync(TEntity newEntity);

		//Delete Entity from database
		Task<bool> DeleteAsync(TEntity entity);
	}
}