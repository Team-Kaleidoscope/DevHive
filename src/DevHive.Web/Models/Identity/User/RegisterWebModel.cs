using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Attributes;

namespace DevHive.Web.Models.Identity.User
{
	public class RegisterWebModel : BaseUserWebModel
	{
		[NotNull]
		[Required]
		[GoodPassword]
		public string Password { get; set; }
	}
}
