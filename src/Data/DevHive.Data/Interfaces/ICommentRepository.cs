using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces
{
	public interface ICommentRepository : IRepository<Comment>
	{
		Task<List<Comment>> GetPostComments(Guid postId);

		Task<bool> DoesCommentExist(Guid id);
		Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated);
	}
}
