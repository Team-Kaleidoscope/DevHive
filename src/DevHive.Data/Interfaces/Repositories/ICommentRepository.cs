using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface ICommentRepository : IRepository<Comment>
	{
		Task<bool> DoesCommentExist(Guid id);
		Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated);
	}
}
