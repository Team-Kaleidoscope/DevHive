using System.Collections.Generic;
using DevHive.Services.Models.Post;

namespace DevHive.Services.Models
{
	public class ReadPageServiceModel
	{
		public List<ReadPostServiceModel> Posts { get; set; } = new();
	}
}
