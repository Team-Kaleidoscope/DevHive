using System;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class CommentRepositoryTests
	{
		private const string COMMENT_MESSAGE = "Comment message";
		private DevHiveContext _context;
		private CommentRepository _commentRepository;

		#region Setups
		[SetUp]
		public void Setup()
		{
			DbContextOptionsBuilder<DevHiveContext> optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this._context = new DevHiveContext(optionsBuilder.Options);

			this._commentRepository = new CommentRepository(this._context);
		}

		[TearDown]
		public void TearDown()
		{
			this._context.Database.EnsureDeleted();
		}
		#endregion

		#region GetCommentByIssuerAndTimeCreatedAsync
		[Test]
		public async Task GetCommentByCreatorAndTimeCreatedAsync_ReturnsTheCorrectComment_IfItExists()
		{
			Comment comment = await this.AddEntity();

			Comment resultComment = await this._commentRepository.GetCommentByIssuerAndTimeCreatedAsync(comment.Creator.Id, comment.TimeCreated);

			Assert.AreEqual(comment.Id, resultComment.Id, "GetCommentByIssuerAndTimeCreatedAsync does not return the corect comment when it exists");
		}

		[Test]
		public async Task GetPostByCreatorAndTimeCreatedAsync_ReturnsNull_IfThePostDoesNotExist()
		{
			await this.AddEntity();

			Comment resultComment = await this._commentRepository.GetCommentByIssuerAndTimeCreatedAsync(Guid.Empty, DateTime.Now);

			Assert.IsNull(resultComment, "GetCommentByIssuerAndTimeCreatedAsync does not return null when the comment does not exist");
		}
		#endregion

		#region DoesCommentExist
		[Test]
		public async Task DoesCommentExist_ReturnsTrue_WhenTheCommentExists()
		{
			Comment comment = await this.AddEntity();

			bool result = await this._commentRepository.DoesCommentExist(comment.Id);

			Assert.IsTrue(result, "DoesCommentExist does not return true whenm the Comment exists");
		}

		[Test]
		public async Task DoesCommentExist_ReturnsFalse_WhenTheCommentDoesNotExist()
		{
			bool result = await this._commentRepository.DoesCommentExist(Guid.Empty);

			Assert.IsFalse(result, "DoesCommentExist does not return false whenm the Comment" +
				" does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task<Comment> AddEntity()
		{
			User creator = new() { Id = Guid.NewGuid() };
			Comment comment = new()
			{
				Message = COMMENT_MESSAGE,
				Creator = creator,
				TimeCreated = DateTime.Now
			};

			this._context.Comments.Add(comment);
			await this._context.SaveChangesAsync();

			return comment;
		}
		#endregion
	}
}
