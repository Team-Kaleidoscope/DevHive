using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Common.Models.Identity;
using DevHive.Common.Models.Misc;
using DevHive.Services.Models.Identity.User;

namespace DevHive.Services.Interfaces
{
	public interface IUserService
	{
		Task<TokenModel> LoginUser(LoginServiceModel loginModel);
		Task<TokenModel> RegisterUser(RegisterServiceModel registerModel);

		Task<bool> AddFriend(Guid userId, Guid friendId);

		Task<UserServiceModel> GetUserByUsername(string username);
		Task<UserServiceModel> GetUserById(Guid id);

		Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateModel);
		Task<UserServiceModel> PatchUser(Guid id, List<Patch> patch);

		Task DeleteUser(Guid id);
		Task<bool> RemoveFriend(Guid userId, Guid friendId);

		Task<bool> ValidJWT(Guid id, string rawTokenData);
	}
}
