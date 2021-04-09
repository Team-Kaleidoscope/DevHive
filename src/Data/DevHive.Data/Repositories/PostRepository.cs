using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Data.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class PostRepository : BaseRepository<Post>, IPostRepository
	{
		private readonly DevHiveContext _context;
		private readonly IUserRepository _userRepository;

		public PostRepository(DevHiveContext context, IUserRepository userRepository)
			: base(context)
		{
			this._context = context;
			this._userRepository = userRepository;
		}

		public async Task<bool> AddNewPostToCreator(Guid userId, Post post)
		{
			User user = await this._userRepository.GetByIdAsync(userId);
			user.Posts.Add(post);

			return await this.SaveChangesAsync();
		}

		#region Read
		public override async Task<Post> GetByIdAsync(Guid id)
		{
			return await this._context.Posts
					.Include(x => x.Comments)
					.Include(x => x.Creator)
					.Include(x => x.Attachments)
					.Include(x => x.Ratings)
					.FirstOrDefaultAsync(x => x.Id == id);
		}

		/// <summary>
		/// This method returns the post that is made at exactly the given time and by the given creator
		/// </summary>
		public async Task<Post> GetPostByCreatorAndTimeCreatedAsync(Guid creatorId, DateTime timeCreated)
		{
			return await this._context.Posts
				.FirstOrDefaultAsync(p => p.Creator.Id == creatorId &&
					p.TimeCreated == timeCreated);
		}

		public async Task<List<string>> GetFileUrls(Guid postId)
		{
			return (await this.GetByIdAsync(postId)).Attachments.Select(x => x.FileUrl).ToList();
		}
		#endregion

		#region Update
		public override async Task<bool> EditAsync(Guid id, Post newEntity)
		{
			Post post = await this.GetByIdAsync(id);

			this._context
				.Entry(post)
				.CurrentValues
				.SetValues(newEntity);

			List<PostAttachments> postAttachments = new();
			foreach (var attachment in newEntity.Attachments)
				postAttachments.Add(attachment);
			post.Attachments = postAttachments;

			post.Comments.Clear();
			foreach (var comment in newEntity.Comments)
				post.Comments.Add(comment);

			this._context.Entry(post).State = EntityState.Modified;

			return await this.SaveChangesAsync();
		}
		#endregion

		#region Validations
		public async Task<bool> DoesPostExist(Guid postId)
		{
			return await this._context.Posts
				.AsNoTracking()
				.AnyAsync(r => r.Id == postId);
		}

		public async Task<bool> DoesPostHaveFiles(Guid postId)
		{
			return await this._context.Posts
				.AsNoTracking()
				.Where(x => x.Id == postId)
				.Select(x => x.Attachments)
				.AnyAsync();
		}
		#endregion
	}
}
