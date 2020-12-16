using System.ComponentModel.DataAnnotations;
using DevHive.Web.Models.Identity.Validation;

namespace DevHive.Web.Models.Identity.User 
{
	public class UpdateUserWebModel : BaseUserWebModel
	{
		[Required]
		[GoodPassword]
		public virtual string Password { get; set; }
	}
}
