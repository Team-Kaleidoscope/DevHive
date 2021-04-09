using System;

namespace DevHive.Services.Models.Comment
{
	public class CreateCommentServiceModel
	{
		public Guid PostId { get; set; }

		public Guid CreatorId { get; set; }

		public string Message { get; set; }
	}
}
