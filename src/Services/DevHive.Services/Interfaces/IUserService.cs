using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Identity;
using DevHive.Services.Models.User;

namespace DevHive.Services.Interfaces
{
	public interface IUserService
	{
		/// <summary>
		/// Log ins an existing user and gives him/her a JWT Token for further authorization
		/// </summary>
		/// <param name="loginModel">Login service model, conaining user's username and password</param>
		/// <returns>A JWT Token for authorization</returns>
		Task<TokenModel> LoginUser(LoginServiceModel loginModel);

		/// <summary>
		/// Registers a new user and gives him/her a JWT Token for further authorization
		/// </summary>
		/// <param name="registerModel">Register service model, containing the new user's data</param>
		/// <returns>A JWT Token for authorization</returns>
		Task<TokenModel> RegisterUser(RegisterServiceModel registerModel);

		/// <summary>
		/// Get a user by his username. Used for querying profiles without provided authentication
		/// </summary>
		/// <param name="username">User's username, who's to be queried</param>
		/// <returns>The queried user or null, if non existant</returns>
		Task<UserServiceModel> GetUserByUsername(string username);

		/// <summary>
		/// Get a user by his Guid. Used for querying full user's profile
		/// Requires authenticated user
		/// </summary>
		/// <param name="id">User's username, who's to be queried</param>
		/// <returns>The queried user or null, if non existant</returns>
		Task<UserServiceModel> GetUserById(Guid id);

		/// <summary>
		/// Updates a user's data, provided a full model with new details
		/// Requires authenticated user
		/// </summary>
		/// <param name="updateUserServiceModel">Full update user model for updating</param>
		/// <returns>Read model of the new user</returns>
		Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateUserServiceModel);

		/// <summary>
		/// Uploads the given picture and assigns it's link to the user in the database
		/// Requires authenticated user
		/// </summary>
		/// <param name="updateProfilePictureServiceModel">Contains User's Guid and the new picture to be updated</param>
		/// <returns>The new picture's URL</returns>
		Task<ProfilePictureServiceModel> UpdateProfilePicture(UpdateProfilePictureServiceModel updateProfilePictureServiceModel);

		/// <summary>
		/// Deletes a user from the database and removes his data entirely
		/// Requires authenticated user
		/// </summary>
		/// <param name="id">The user's Guid, who's to be deleted</param>
		/// <returns>True if successfull, false otherwise</returns>
		Task<bool> DeleteUser(Guid id);

		/// <summary>
		/// We don't talk about that!
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<TokenModel> SuperSecretPromotionToAdmin(Guid userId);
	}
}
