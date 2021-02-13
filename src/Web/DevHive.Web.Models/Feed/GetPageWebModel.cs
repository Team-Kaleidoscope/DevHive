using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevHive.Web.Models.Feed
{
	public class GetPageWebModel
	{
		[Range(1, int.MaxValue)]
		public int PageNumber { get; set; }

		[Required]
		public DateTime FirstPageTimeIssued { get; set; }

		[DefaultValue(5)]
		[Range(1, int.MaxValue)]
		public int PageSize { get; set; }
	}
}
