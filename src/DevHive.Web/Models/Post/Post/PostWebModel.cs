using DevHive.Web.Models.Post.Comment;

namespace DevHive.Web.Models.Post.Post
{
	public class PostWebModel
	{
		//public string Picture { get; set; }

		public string IssuerFirstName { get; set; }

		public string IssuerLastName { get; set; }

		public string IssuerUsername { get; set; }

		public string Message { get; set; }

		//public Files[] Files { get; set; }

		public CommentWebModel[] Comments { get; set; }
	}
}