using System;

namespace DevHive.Services.Models.Post.Comment
{
	public class CreateCommentServiceModel : BaseCommentServiceModel
	{
		public DateTime TimeCreated { get; set; }
	}
}