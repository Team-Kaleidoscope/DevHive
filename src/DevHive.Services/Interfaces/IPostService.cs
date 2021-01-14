using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Post.Comment;
using DevHive.Services.Models.Post.Post;

namespace DevHive.Services.Interfaces
{
	public interface IPostService
	{
		Task<bool> CreatePost(CreatePostServiceModel postServiceModel);
		Task<bool> AddComment(CreateCommentServiceModel commentServiceModel);

		Task<CommentServiceModel> GetCommentById(Guid id);
		Task<PostServiceModel> GetPostById(Guid id);

		Task<bool> UpdateComment(UpdateCommentServiceModel commentServiceModel);
		Task<bool> UpdatePost(UpdatePostServiceModel postServiceModel);

		Task<bool> DeleteComment(Guid id);
		Task<bool> DeletePost(Guid id);

		Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData);
	}
}
