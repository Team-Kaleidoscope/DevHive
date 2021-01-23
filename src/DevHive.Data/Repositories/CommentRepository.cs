using System;
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
		public async Task<Comment> GetCommentByIssuerAndTimeCreatedAsync(Guid issuerId, DateTime timeCreated)
		{
			return await this._context.Comments
				.FirstOrDefaultAsync(p => p.CreatorId == issuerId &&
					p.TimeCreated == timeCreated);
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
