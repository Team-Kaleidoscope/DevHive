using System;
using System.Collections.Generic;

namespace DevHive.Services.Models.Identity.User
{
    public class UpdateUserServiceModel : BaseUserServiceModel
	{
		public Guid Id { get; set; }

		public HashSet<UpdateUserCollectionServiceModel> Roles { get; set; } = new HashSet<UpdateUserCollectionServiceModel>();

		public HashSet<UpdateUserCollectionServiceModel> Friends { get; set; } = new HashSet<UpdateUserCollectionServiceModel>();

		public HashSet<UpdateUserCollectionServiceModel> Languages { get; set; } = new HashSet<UpdateUserCollectionServiceModel>();

		public HashSet<UpdateUserCollectionServiceModel> Technologies { get; set; } = new HashSet<UpdateUserCollectionServiceModel>();

	}
}
