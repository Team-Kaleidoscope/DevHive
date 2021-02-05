using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface IFeedRepository
	{
		Task<List<Post>> GetFriendsPosts(List<User> friendsList, DateTime firstRequestIssued, int pageNumber, int pageSize);
		Task<List<Post>> GetUsersPosts(User user, DateTime firstRequestIssued, int pageNumber, int pageSize);
	}
}
