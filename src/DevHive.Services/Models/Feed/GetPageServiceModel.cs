using System;

namespace DevHive.Services.Models
{
	public class GetPageServiceModel
	{
		public Guid UserId { get; set; }

		public int PageNumber { get; set; }

		public DateTime FirstRequestIssued { get; set; }

		public int PageSize { get; set; }
	}
}
