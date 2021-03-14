using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class RatingRepository : BaseRepository<Rating>, IRatingRepository
	{
		private readonly DevHiveContext _context;
		private readonly IPostRepository _postRepository;

		public RatingRepository(DevHiveContext context, IPostRepository postRepository)
			: base(context)
		{
			this._context = context;
			this._postRepository = postRepository;
		}

		public override async Task<Rating> GetByIdAsync(Guid id)
		{
			return await this._context.Rating
				.Include(x => x.User)
				.Include(x => x.Post)
				.FirstOrDefaultAsync(x => x.Id == id);
		}
		/// <summary>
		/// Gets all the ratings for a psot.
		/// </summary>
		/// <param name="postId">Id of the post.</param>
		/// <returns></returns>
		public async Task<List<Rating>> GetRatingsByPostId(Guid postId)
		{
			return await this._context.Rating
				.Include(x => x.User)
				.Include(x => x.Post)
				.Where(x => x.Post.Id == postId).ToListAsync();
		}
		/// <summary>
		/// Checks if a user rated a given post.
		/// </summary>
		/// <param name="userId">Id of the user.</param>
		/// <param name="postId">Id of the psot.</param>
		/// <returns>True if the user has already rated the post and false if he hasn't.</returns>
		public async Task<bool> UserRatedPost(Guid userId, Guid postId)
		{
			return await this._context.Rating
				.Where(x => x.Post.Id == postId)
				.AnyAsync(x => x.User.Id == userId);
		}
		/// <summary>
		/// Gets a rating by the post to which the rating corresponds and the user who created it.
		/// </summary>
		/// <param name="userId">Id of the user.</param>
		/// <param name="postId">Id of the post.</param>
		/// <returns>Rating for the given post by the given user.</returns>
		public async Task<Rating> GetRatingByUserAndPostId(Guid userId, Guid postId)
		{
			return await this._context.Rating
				.Include(x => x.User)
				.Include(x => x.Post)
				.FirstOrDefaultAsync(x => x.Post.Id == postId && x.User.Id == userId);
		}

		/// <summary>
		/// Checks if a given rating already exist
		/// </summary>
		/// <param name="id">Id of the rating</param>
		/// <returns>True if the rating exists and false if it does not.</returns>
		public async Task<bool> DoesRatingExist(Guid id)
		{
			return await this._context.Rating
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
	}
}

