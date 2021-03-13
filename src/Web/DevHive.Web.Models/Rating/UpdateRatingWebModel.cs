using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHive.Web.Models.Rating
{
	public class UpdateRatingWebModel
	{
		public Guid Id { get; set; }

		public Guid PostId { get; set; }

		public bool IsLike { get; set; }
	}
}
