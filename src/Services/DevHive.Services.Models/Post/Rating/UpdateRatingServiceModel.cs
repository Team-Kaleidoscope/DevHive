using System;

namespace DevHive.Services.Models.Post.Rating
{
	public class UpdateRatingServiceModel
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public bool IsLike { get; set; }
	}
}
