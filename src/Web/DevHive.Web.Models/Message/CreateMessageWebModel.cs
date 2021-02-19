using System;

namespace DevHive.Web.Models.Message
{
	public class CreateMessageWebModel
	{
		public string Content { get; set; }

		public Guid ChatId { get; set; }
	}
}
