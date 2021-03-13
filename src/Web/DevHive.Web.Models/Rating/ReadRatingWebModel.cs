using System;

namespace DevHive.Web.Models.Rating
{
	public class ReadRatingWebModel
	{
		public Guid Id { get; set; }

		public Guid PostId { get; set; }

		public Guid UserId { get; set; }

		public bool IsLike { get; set; }
	}
}
