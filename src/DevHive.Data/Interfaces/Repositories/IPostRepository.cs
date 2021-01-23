using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<Post> GetPostByCreatorAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated);
		Task<bool> DoesPostExist(Guid postId);
	}
}
