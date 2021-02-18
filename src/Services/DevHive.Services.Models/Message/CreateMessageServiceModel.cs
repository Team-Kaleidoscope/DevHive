using System;

namespace DevHive.Services.Models.Message
{
	public class CreateMessageServiceModel
	{
		public string Content { get; set; }

		public Guid CreatorId { get; set; }

		public Guid ChatId { get; set; }
	}
}
