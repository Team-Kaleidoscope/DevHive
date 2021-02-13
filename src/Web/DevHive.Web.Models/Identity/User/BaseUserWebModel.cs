using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Attributes;

namespace DevHive.Web.Models.Identity.User
{
	public class BaseUserWebModel
	{
		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string UserName { get; set; }

		[NotNull]
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		[OnlyLetters(ErrorMessage = "First name can only contain letters!")]
		public string FirstName { get; set; }

		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		[OnlyLetters(ErrorMessage = "Last name can only contain letters!")]
		public string LastName { get; set; }
	}
}
