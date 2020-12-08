using System.Collections.Generic;
using System.Threading.Tasks;

namespace Camera_Shop.Database
{
	public interface IRepository<TEntity>
		where TEntity : class
	{
		//Add Entity to database
		Task AddAsync(TEntity entity);

		//Return *count* instances of Entity from the database
		Task<IAsyncEnumerable<TEntity>> Query(int count);

		//Find entity by id
		Task<TEntity> FindByIdAsync(object id);

		//Modify Entity from database
		Task EditAsync(object id, TEntity newEntity);

		//Delete Entity from database
		Task DeleteAsync(object id);
	}
}