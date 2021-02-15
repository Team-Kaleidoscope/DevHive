using System;
using System.Collections.Generic;
using DevHive.Common.Models.Misc;

namespace DevHive.Web.Models.Post
{
	public class ReadPostWebModel
	{
		public Guid PostId { get; set; }

		public string CreatorFirstName { get; set; }

		public string CreatorLastName { get; set; }

		public string CreatorUsername { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		public List<IdModel> Comments { get; set; }

		public List<string> FileUrls { get; set; }
	}
}
