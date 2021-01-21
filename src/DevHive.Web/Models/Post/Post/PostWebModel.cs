using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Attributes;
using DevHive.Web.Models.Post.Comment;

namespace DevHive.Web.Models.Post.Post
{
	public class PostWebModel
	{
		//public string Picture { get; set; }

		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string IssuerFirstName { get; set; }

		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string IssuerLastName { get; set; }

		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		[OnlyAlphanumerics(ErrorMessage = "Username can only contain letters and digits!")]
		public string IssuerUsername { get; set; }

		[NotNull]
		[Required]
		public string Message { get; set; }

		//public Files[] Files { get; set; }

		public CommentWebModel[] Comments { get; set; }
	}
}
