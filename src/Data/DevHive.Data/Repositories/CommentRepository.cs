using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHive.Data.Repositories
{
	public class CommentRepository : BaseRepository<Comment>, ICommentRepository
	{
		private readonly DevHiveContext _context;

		public CommentRepository(DevHiveContext context)
			: base(context)
		{
			this._context = context;
		}

		#region Read
		public override async Task<Comment> GetByIdAsync(Guid id)
		{
			return await this._context.Comments
				.Include(x => x.Creator)
				.Include(x => x.Post)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		/// <summary>
        /// This method returns the comment that is made at exactly the given time and by the given creator
        /// </summary>
		public async Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated)
		{
			return await this._context.Comments
				.FirstOrDefaultAsync(p => p.Creator.Id == issuerId &&
					p.TimeCreated == timeCreated);
		}

		public async Task<List<Comment>> GetPostComments(Guid postId)
		{
			return await this._context.Posts
				.SelectMany(x => x.Comments)
				.Where(x => x.Post.Id == postId)
				.ToListAsync();
		}
		#endregion

		#region Update
		public override async Task<bool> EditAsync(Guid id, Comment newEntity)
		{
			Comment comment = await this.GetByIdAsync(id);

			this._context
				.Entry(comment)
				.CurrentValues
				.SetValues(newEntity);

			return await this.SaveChangesAsync();
		}
		#endregion


		#region Validations
		public async Task<bool> DoesCommentExist(Guid id)
		{
			return await this._context.Comments
				.AsNoTracking()
				.AnyAsync(r => r.Id == id);
		}
		#endregion
	}
}
