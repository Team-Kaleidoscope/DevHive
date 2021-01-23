using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Post.Comment;
using DevHive.Services.Models.Post.Post;

namespace DevHive.Services.Interfaces
{
	public interface IPostService
	{
		Task<Guid> CreatePost(CreatePostServiceModel createPostServiceModel);
		Task<Guid> AddComment(CreateCommentServiceModel createPostServiceModel);

		Task<ReadPostServiceModel> GetPostById(Guid id);
		Task<ReadCommentServiceModel> GetCommentById(Guid id);

		Task<Guid> UpdatePost(UpdatePostServiceModel updatePostServiceModel);
		Task<Guid> UpdateComment(UpdateCommentServiceModel updateCommentServiceModel);

		Task<bool> DeletePost(Guid id);
		Task<bool> DeleteComment(Guid id);

		Task<bool> ValidateJwtForPost(Guid postId, string rawTokenData);
		Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData);
	}
}
