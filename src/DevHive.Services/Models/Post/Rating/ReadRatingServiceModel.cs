using System;

namespace DevHive.Services.Models.Post.Rating
{
	public class ReadRatingServiceModel
	{
		public Guid PostId { get; set; }

		public int Likes { get; set; }

		public int Dislikes { get; set; }
	}
}
