using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class UpdateCommentServiceModel : BaseCommentServiceModel
	{
		public Guid CommentId { get; set; }
	}
}
