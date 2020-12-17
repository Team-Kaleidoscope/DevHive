using System;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Common.Models.Data;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class FriendsRepository : UserRepository
	{
		private readonly DbContext _context;

		public FriendsRepository(DbContext context) 
			: base(context)
		{
			this._context = context;
		}
	
		//Create
		public async Task<bool> AddFriendAsync(User user, User friend)
		{
			this._context.Update(user);
			user.Friends.Add(friend);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}

		//Delete
		public async Task<bool> RemoveFriendAsync(User user, User friend)
		{
			this._context.Update(user);
			user.Friends.Remove(friend);

			return await RepositoryMethods.SaveChangesAsync(this._context);
		}
	}
}