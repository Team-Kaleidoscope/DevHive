using System;

namespace DevHive.Services.Models.Rating
{
	public class CreateRatingServiceModel
	{
		public Guid UserId { get; set; }

		public Guid PostId { get; set; }

		public bool IsLike { get; set; }
	}
}
