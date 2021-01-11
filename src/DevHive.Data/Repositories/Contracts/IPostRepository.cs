using DevHive.Data.Models;
using System;
using System.Threading.Tasks;

namespace DevHive.Data.Repositories.Contracts
{
    public interface IPostRepository : IRepository<Post>
    {
        public Task<bool> AddCommentAsync(Comment entity);

        public Task<Comment> GetCommentByIdAsync(Guid id);

        public Task<bool> EditCommentAsync(Comment newEntity);

		public Task<bool> DeleteCommentAsync(Comment entity);

		public Task<bool> DoesPostExist(Guid postId);

        public Task<bool> DoesCommentExist(Guid id);
	}
}
