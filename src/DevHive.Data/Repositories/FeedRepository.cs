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

		/// <summary>
        /// This returns a given amount of posts of all given friends, created before "firstRequestIssued",
		/// ordered from latest to oldest (time created).
		/// PageSize specifies how many posts to get, and pageNumber specifices how many posts to skip (pageNumber * pageSize).
		///
		/// This method is used in the feed page.
		/// Posts from friends are meant to be gotten in chunks, meaning you get X posts, and then get another amount of posts,
		/// that are after the first X posts.
        /// </summary>
		public async Task<List<Post>> GetFriendsPosts(List<User> friendsList, DateTime firstRequestIssued, int pageNumber, int pageSize)
		{
			List<Guid> friendsIds = friendsList.Select(f => f.Id).ToList();

			List<Post> posts = await this._context.Posts
				.Where(post => post.TimeCreated < firstRequestIssued)
				.Where(p => friendsIds.Contains(p.Creator.Id))
				.ToListAsync();

			// Ordering by descending can't happen in query, because it doesn't order it
			// completely correctly (example: in query these two times are ordered
			// like this: 2021-01-30T11:49:45, 2021-01-28T21:37:40.701244)
			posts = posts
				.OrderByDescending(x => x.TimeCreated.ToFileTime())
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			return posts;
		}

		/// <summary>
        /// This returns a given amount of posts, that a user has made, created before "firstRequestIssued",
		/// ordered from latest to oldest (time created).
		/// PageSize specifies how many posts to get, and pageNumber specifices how many posts to skip (pageNumber * pageSize).
		///
		/// This method is used in the profile page.
		/// Posts from friends are meant to be gotten in chunks, meaning you get X posts, and then get another amount of posts,
		/// that are after the first X posts.
        /// </summary>
		public async Task<List<Post>> GetUsersPosts(User user, DateTime firstRequestIssued, int pageNumber, int pageSize)
		{
			List<Post> posts = await this._context.Posts
				.Where(post => post.TimeCreated < firstRequestIssued)
				.Where(p => p.Creator.Id == user.Id)
				.ToListAsync();

			// Look at GetFriendsPosts on why this is done like this
			posts = posts
				.OrderByDescending(x => x.TimeCreated.ToFileTime())
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			return posts;
		}
	}
}
