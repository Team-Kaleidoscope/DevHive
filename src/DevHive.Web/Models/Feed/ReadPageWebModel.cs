using System.Collections.Generic;
using DevHive.Web.Models.Post.Post;

namespace DevHive.Web.Controllers
{
	public class ReadPageWebModel
	{
		public List<ReadPostWebModel> Posts { get; set; } = new();
	}
}
