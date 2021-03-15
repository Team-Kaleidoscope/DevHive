using System;

namespace DevHive.Services.Models.Rating
{
	public class ReadRatingServiceModel
	{
		public Guid Id { get; set; }

		public Guid PostId { get; set; }

		public Guid UserId { get; set; }

		public bool IsLike { get; set; }
	}
}
