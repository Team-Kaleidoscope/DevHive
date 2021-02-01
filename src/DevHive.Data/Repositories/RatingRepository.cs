using System;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
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

		public async Task<Rating> GetByPostId(Guid postId)
		{
			throw new NotImplementedException();
			// return await this._context.Rating
			// 	.FirstOrDefaultAsync(x => x.Post.Id == postId);
		}

		public async Task<int> GetRating(Guid postId)
		{
			throw new NotImplementedException();
			// return (await this.GetByPostId(postId)).Rate;
		}
	}
}
