using System;

namespace DevHive.Services.Models.Rating
{
	public class UpdateRatingServiceModel
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public Guid PostId { get; set; }

		public bool IsLike { get; set; }
	}
}
