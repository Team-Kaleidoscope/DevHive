using System;
using System.Threading.Tasks;

namespace Data.Models.Interfaces.Database
{
	public interface IRepository<TEntity>
		where TEntity : class
	{
		//Add Entity to database
		Task AddAsync(TEntity entity);

		//Find entity by id
		Task<TEntity> GetByIdAsync(Guid id);

		//Modify Entity from database
		Task EditAsync(TEntity newEntity);

		//Delete Entity from database
		Task DeleteAsync(TEntity entity);
	}
}