namespace DevHive.Services.Models.Post.Comment
{
	public class ReadCommentServiceModel : BaseCommentServiceModel
	{
		public string IssuerFirstName { get; set; }

		public string IssuerLastName { get; set; }

		public string IssuerUsername { get; set; }
	}
}
