using System.Collections.Generic;
using DevHive.Web.Models.Post;

namespace DevHive.Web.Models.Comment
{
	public class ReadPageWebModel
	{
		public List<ReadPostWebModel> Posts { get; set; } = new();
	}
}
