using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DevHive.Web.Models.Attributes;

namespace DevHive.Web.Models.User
{
	public class UsernameWebModel
	{
		[NotNull]
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string UserName { get; set; }
	}
}
