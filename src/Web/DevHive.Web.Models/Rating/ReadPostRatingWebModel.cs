using System;

namespace DevHive.Web.Models.Rating
{
	public class ReadPostRatingWebModel
	{
		public Guid Id { get; set; }

		public Guid PostId { get; set; }

		public int Likes { get; set; }

		public int Dislikes { get; set; }
	}
}
