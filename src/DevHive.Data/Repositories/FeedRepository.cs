using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class FeedRepository : IFeedRepository
	{
		private readonly DevHiveContext _context;

		public FeedRepository(DevHiveContext context)
		{
			this._context = context;
		}
		public async Task<List<Post>> GetFriendsPosts(List<User> friendsList, DateTime firstRequestIssued, int pageNumber, int pageSize)
		{
			List<Guid> friendsIds = friendsList.Select(f => f.Id).ToList();

			List<Post> posts = await this._context.Posts
				.Where(post => post.TimeCreated < firstRequestIssued)
				.Where(p => friendsIds.Contains(p.CreatorId))
				.OrderByDescending(x => x.TimeCreated)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return posts;
		}
	}
}
