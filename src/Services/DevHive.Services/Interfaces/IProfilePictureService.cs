using System;
using System.Threading.Tasks;
using DevHive.Services.Models.ProfilePicture;

namespace DevHive.Services.Interfaces
{
	public interface IProfilePictureService
	{
		Task<string> InsertProfilePicture(ProfilePictureServiceModel profilePictureServiceModel);

		Task<string> GetProfilePictureById(Guid id);

		/// <summary>
		/// Uploads the given picture and assigns it's link to the user in the database
		/// </summary>
		/// <param name="profilePictureServiceModel">Contains User's Guid and the new picture to be updated</param>
		/// <returns>The new picture's URL</returns>
		Task<string> UpdateProfilePicture(ProfilePictureServiceModel profilePictureServiceModel);

		Task<bool> DeleteProfilePicture(Guid id);
	}
}
