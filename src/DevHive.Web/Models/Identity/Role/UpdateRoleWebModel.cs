using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevHive.Web.Models.Identity.Role
{
	public class UpdateRoleWebModel : RoleWebModel
	{
		[NotNull]
		[Required]
		public Guid Id { get; set; }
	}
}
