using System;

namespace DevHive.Web.Models.Message
{
	public class ReadMessageWebModel
	{
		public Guid Id { get; set; }

		public string CreatorFirstName { get; set; }

		public string CreatorLastName { get; set; }

		public string CreatorUsername { get; set; }

		public DateTime TimeCreated { get; set; }

		public string Content { get; set; }
	}
}
