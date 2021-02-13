using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Models.Attributes;

namespace DevHive.Web.Models.User
{
	public class RegisterWebModel : BaseUserWebModel
	{
		[NotNull]
		[Required]
		[GoodPassword]
		public string Password { get; set; }
	}
}
