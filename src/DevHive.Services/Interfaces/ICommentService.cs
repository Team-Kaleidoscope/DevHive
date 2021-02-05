using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Comment;

namespace DevHive.Services.Interfaces
{
	public interface ICommentService
	{
		Task<Guid> AddComment(CreateCommentServiceModel createPostServiceModel);

		Task<ReadCommentServiceModel> GetCommentById(Guid id);

		Task<Guid> UpdateComment(UpdateCommentServiceModel updateCommentServiceModel);

		Task<bool> DeleteComment(Guid id);

		Task<bool> ValidateJwtForCreating(Guid userId, string rawTokenData);
		Task<bool> ValidateJwtForComment(Guid commentId, string rawTokenData);
	}
}
