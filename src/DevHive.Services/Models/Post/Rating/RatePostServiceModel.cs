using System;

namespace DevHive.Services.Models.Post.Rating
{
	public class RatePostServiceModel
	{
		public Guid UserId { get; set; }

		public Guid PostId { get; set; }

		public bool Liked { get; set; }
	}
}
