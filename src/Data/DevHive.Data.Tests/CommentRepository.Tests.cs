using System;
using System.Collections.Generic;
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

		#region SetUp
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

		#region GetByIdAsync
		[Test]
		public async Task GetByIdAsync_ReturnsTheCorrectComment_IfItExists()
		{
			Comment comment = await this.AddEntity();

			Comment resultComment = await this._commentRepository.GetByIdAsync(comment.Id);

			Assert.AreEqual(comment.Id, resultComment.Id, "GetByIdAsync does not return the correct comment when it exists.");
		}

		[Test]
		public async Task GetByIdAsync_ReturnsNull_IfCommentDoesNotExist()
		{
			Comment resultComment = await this._commentRepository.GetByIdAsync(Guid.Empty);

			Assert.IsNull(resultComment, "GetByIdAsync does not return null when the comment does not exist");
		}
		#endregion

		#region GetPostComments
		[Test]
		public async Task GetPostComments_ReturnsAllCommentsForPost_IfAnyExist()
		{
			List<Comment> comments = new List<Comment>
			{
				new Comment(),
				new Comment(),
				new Comment()
			};
			Post post = new Post
			{
				Id = Guid.NewGuid(),
				Comments = comments
			};

			this._context.Posts.Add(post);
			await this._context.SaveChangesAsync();

			List<Comment> resultComments = await this._commentRepository.GetPostComments(post.Id);

			Assert.AreEqual(comments.Count, resultComments.Count, "GetPostComments does not return the comments for a given post correctly");
		}

		[Test]
		public async Task GetPostComments_ReturnsEmptyList_WhenPostDoesNotExist()
		{
			List<Comment> resultComments = await this._commentRepository.GetPostComments(Guid.Empty);

			Assert.IsEmpty(resultComments, "GetPostComments does not return empty string when post does not exist");
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
			Comment resultComment = await this._commentRepository.GetCommentByIssuerAndTimeCreatedAsync(Guid.Empty, DateTime.Now);

			Assert.IsNull(resultComment, "GetCommentByIssuerAndTimeCreatedAsync does not return null when the comment does not exist");
		}
		#endregion

		#region EditAsync
		[Test]
		public async Task EditAsync_ReturnsTrue_WhenCommentIsUpdatedSuccessfully()
		{
			string newMessage = "New message!";
			Comment comment = await this.AddEntity();
			Comment updatedComment = new Comment
			{
				Id = comment.Id,
				Message = newMessage
			};

			bool result = await this._commentRepository.EditAsync(comment.Id, updatedComment);

			Assert.IsTrue(result, "EditAsync does not return true when comment is updated successfully");
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
				Id = Guid.NewGuid(),
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
