using System;
using DevHive.Services.Models.User;

namespace DevHive.Services.Models.Message
{
	public class ReadMessageServiceModel
	{
		public Guid Id { get; set; }

		public Guid ChatId { get; set; }

		public string Content { get; set; }

		public DateTime TimeCreated { get; set; }

		public UserServiceModel Creator { get; set; }
	}
}
