using System.ComponentModel.DataAnnotations;
using DevHive.Web.Models.Identity.Validation;

namespace DevHive.Web.Models.Identity.User 
{
	public class RegisterWebModel 
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		[OnlyAlphanumerics(ErrorMessage = "Username can only contain letters and digits!")]
		public string UserName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		[OnlyLetters(ErrorMessage = "First name can only contain letters!")]
		public string FirstName { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(30)]
		[OnlyLetters(ErrorMessage = "Last name can only contain letters!")]
		public string LastName { get; set; }

		[Required]
		[GoodPassword]
		public string Password { get; set; }
	}
}
