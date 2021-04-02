using System;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces
{
	public interface IProfilePictureRepository : IRepository<ProfilePicture>
	{
		Task<ProfilePicture> GetByURLAsync(string picUrl);
	}
}
