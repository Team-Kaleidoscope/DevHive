using System;

namespace DevHive.Web.Models.Rating
{
	public class RatePostWebModel
	{
		public Guid PostId { get; set; }

		public bool Liked { get; set; }
	}
}
