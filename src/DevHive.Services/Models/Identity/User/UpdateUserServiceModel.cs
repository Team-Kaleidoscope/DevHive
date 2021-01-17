using System;
using System.Collections.Generic;

namespace DevHive.Services.Models.Identity.User
{
    public class UpdateUserServiceModel : BaseUserServiceModel
	{
		public Guid Id { get; set; }

		public IList<UpdateUserCollectionServiceModel> Roles { get; set; } = new List<UpdateUserCollectionServiceModel>();

		public IList<UpdateUserCollectionServiceModel> Friends { get; set; } = new List<UpdateUserCollectionServiceModel>();

		public IList<UpdateUserCollectionServiceModel> Languages { get; set; } = new List<UpdateUserCollectionServiceModel>();

		public IList<UpdateUserCollectionServiceModel> Technologies { get; set; } = new List<UpdateUserCollectionServiceModel>();

	}
}
