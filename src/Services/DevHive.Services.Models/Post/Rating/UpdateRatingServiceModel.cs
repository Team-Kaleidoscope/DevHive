using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHive.Services.Models.Post.Rating
{
	public class UpdateRatingServiceModel
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public bool IsLike { get; set; }
	}
}
