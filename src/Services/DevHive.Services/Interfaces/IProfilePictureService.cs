using System;
using System.Threading.Tasks;
using DevHive.Services.Models.ProfilePicture;

namespace DevHive.Services.Interfaces
{
	public interface IProfilePictureService
	{
		/// <summary>
		/// Get a profile picture by it's Guid
		/// </summary>
		/// <param name="id">Profile picture's Guid</param>
		/// <returns>The profile picture's URL in the cloud</returns>
		Task<string> GetProfilePictureById(Guid id);

		/// <summary>
		/// Uploads the given picture and assigns it's link to the user in the database
		/// </summary>
		/// <param name="profilePictureServiceModel">Contains User's Guid and the new picture to be updated</param>
		/// <returns>The new profile picture's URL in the cloud</returns>
		Task<string> UpdateProfilePicture(ProfilePictureServiceModel profilePictureServiceModel);

		/// <summary>
		/// Delete a profile picture from the cloud and the database
		/// </summary>
		/// <param name="id">The profile picture's Guid</param>
		/// <returns>True if the picture is deleted, false otherwise</returns>
		Task<bool> DeleteProfilePicture(Guid id);
	}
}
