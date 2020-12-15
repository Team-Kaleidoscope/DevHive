using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Models.Interfaces.Database
{
	public interface IRepository<TEntity>
		where TEntity : class
	{
		//Add Entity to database
		Task AddAsync(TEntity entity);

		//Return *count* instances of Entity from the database
		IEnumerable<TEntity> Query(int count);

		//Find entity by id
		Task<TEntity> FindByIdAsync(Guid id);

		//Modify Entity from database
		Task EditAsync(Guid id, TEntity newEntity);

		//Delete Entity from database
		Task DeleteAsync(Guid id);
	}
}