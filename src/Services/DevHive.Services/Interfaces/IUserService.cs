using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Identity;
using DevHive.Services.Models.Identity.User;

namespace DevHive.Services.Interfaces
{
	public interface IUserService
	{
		Task<TokenModel> LoginUser(LoginServiceModel loginModel);
		Task<TokenModel> RegisterUser(RegisterServiceModel registerModel);

		Task<UserServiceModel> GetUserByUsername(string username);
		Task<UserServiceModel> GetUserById(Guid id);

		Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateModel);
		Task<ProfilePictureServiceModel> UpdateProfilePicture(UpdateProfilePictureServiceModel updateProfilePictureServiceModel);

		Task<bool> DeleteUser(Guid id);

		Task<bool> ValidJWT(Guid id, string rawTokenData);

		Task<TokenModel> SuperSecretPromotionToAdmin(Guid userId);
	}
}
