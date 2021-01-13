using System;
using System.Threading.Tasks;
using DevHive.Common.Models.Identity;
using DevHive.Services.Models.Identity.User;
using DevHive.Services.Models.Language;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Interfaces
{
	public interface IUserService
	{
		Task<TokenModel> LoginUser(LoginServiceModel loginModel);
		Task<TokenModel> RegisterUser(RegisterServiceModel registerModel);

		Task<bool> AddFriend(Guid userId, Guid friendId);
		Task<bool> AddLanguageToUser(Guid userId, LanguageServiceModel languageServiceModel);
		Task<bool> AddTechnologyToUser(Guid userId, TechnologyServiceModel technologyServiceModel);

		Task<UserServiceModel> GetFriendById(Guid friendId);
		Task<UserServiceModel> GetUserById(Guid id);

		Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateModel);
		
		Task DeleteUser(Guid id);
		Task<bool> RemoveFriend(Guid userId, Guid friendId);
		Task<bool> RemoveLanguageFromUser(Guid userId, LanguageServiceModel languageServiceModel);
		Task<bool> RemoveTechnologyFromUser(Guid userId, TechnologyServiceModel technologyServiceModel);
		
		Task<bool> ValidJWT(Guid id, string rawTokenData);
	}
}