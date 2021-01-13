using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<bool> AddCommentAsync(Comment entity);

		Task<Comment> GetCommentByIdAsync(Guid id);
		
		Task<bool> EditCommentAsync(Comment newEntity);

		Task<bool> DeleteCommentAsync(Comment entity);
		Task<bool> DoesCommentExist(Guid id);
		Task<bool> DoesPostExist(Guid postId);
	}
} 