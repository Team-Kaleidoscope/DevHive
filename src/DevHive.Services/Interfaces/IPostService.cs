using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Post;

namespace DevHive.Services.Interfaces
{
    public interface IPostService
	{
		Task<Guid> CreatePost(CreatePostServiceModel createPostServiceModel);

		Task<ReadPostServiceModel> GetPostById(Guid id);

		Task<Guid> UpdatePost(UpdatePostServiceModel updatePostServiceModel);

		Task<bool> DeletePost(Guid id);

		Task<bool> ValidateJwtForCreating(Guid userId, string rawTokenData);
		Task<bool> ValidateJwtForPost(Guid postId, string rawTokenData);
	}
}
