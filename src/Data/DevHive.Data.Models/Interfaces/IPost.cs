using System;
using System.Collections.Generic;
using DevHive.Data.Models;
using DevHive.Data.RelationModels;

namespace DevHive.Data.Models.Interfaces
{
	public interface IPost : IModel
	{
		User Creator { get; set; }

		string Message { get; set; }

		DateTime TimeCreated { get; set; }

		List<Comment> Comments { get; set; }

		// Rating Rating { get; set; }

		List<PostAttachments> Attachments { get; set; }
	}
}
