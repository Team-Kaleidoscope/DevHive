using DevHive.Data.Interfaces.Models;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Models
{
	public interface IRating : IModel
	{
		Post Post { get; set; }

		int Likes { get; set; }

		int Dislikes { get; set; }
	}
}
