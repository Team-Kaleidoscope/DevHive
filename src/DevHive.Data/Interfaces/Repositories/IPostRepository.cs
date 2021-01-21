using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<bool> AddCommentAsync(Comment entity);

		Task<Post> GetPostByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated);

		Task<Comment> GetCommentByIdAsync(Guid id);
		Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated);

		Task<bool> EditCommentAsync(Comment newEntity);

		Task<bool> DeleteCommentAsync(Comment entity);
		Task<bool> DoesCommentExist(Guid id);

		Task<bool> DoesPostExist(Guid postId);
	}
}
