using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces
{
	public interface IPostRepository : IRepository<Post>
	{
		Task<bool> AddNewPostToCreator(Guid userId, Post post);

		Task<Post> GetPostByCreatorAndTimeCreatedAsync(Guid creatorId, DateTime timeCreated);
		Task<List<string>> GetFileUrls(Guid postId);

		Task<bool> DoesPostExist(Guid postId);
		Task<bool> DoesPostHaveFiles(Guid postId);
	}
}
