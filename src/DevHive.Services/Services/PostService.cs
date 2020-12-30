using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Post.Comment;
using DevHive.Services.Models.Post.Post;

namespace DevHive.Services.Services
{
	public class PostService
	{
		private readonly PostRepository _postRepository;
		private readonly IMapper _postMapper;

		public PostService(PostRepository postRepository, IMapper postMapper)
		{
			this._postRepository = postRepository;
			this._postMapper = postMapper;
		}

		//Create
		public async Task<bool> CreatePost(CreatePostServiceModel postServiceModel)
		{
			Post post = this._postMapper.Map<Post>(postServiceModel);

			return await this._postRepository.AddAsync(post);
		}

		public async Task<bool> AddComment(CreateCommentServiceModel commentServiceModel)
		{
			commentServiceModel.TimeCreated = DateTime.Now;
			Comment comment = this._postMapper.Map<Comment>(commentServiceModel);
			
			bool result = await this._postRepository.AddCommentAsync(comment);

			return result;
		}
	
		//Read
		public async Task<PostServiceModel> GetPostById(Guid id)
		{
			Post post = await this._postRepository.GetByIdAsync(id) 
				?? throw new ArgumentException("Post does not exist!");

			return this._postMapper.Map<PostServiceModel>(post);
		}

		public async Task<CommentServiceModel> GetCommentById(Guid id)
		{
			Comment comment = await this._postRepository.GetCommentByIdAsync(id);

			if(comment == null)
				throw new ArgumentException("The comment does not exist");

			return this._postMapper.Map<CommentServiceModel>(comment);
		}

		//Update
		public async Task<bool> UpdatePost(UpdatePostServiceModel postServiceModel)
		{
			if (!await this._postRepository.DoesPostExist(postServiceModel.IssuerId))
				throw new ArgumentException("Comment does not exist!");

			Post post = this._postMapper.Map<Post>(postServiceModel);
			return await this._postRepository.EditAsync(post);
		}

		public async Task<bool> UpdateComment(UpdateCommentServiceModel commentServiceModel)
		{
			if (!await this._postRepository.DoesCommentExist(commentServiceModel.Id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = this._postMapper.Map<Comment>(commentServiceModel);
			bool result = await this._postRepository.EditCommentAsync(comment);

			return result;
		}

		//Delete
		public async Task<bool> DeletePost(Guid id)
		{
			Post post = await this._postRepository.GetByIdAsync(id);
			return await this._postRepository.DeleteAsync(post);
		}
	
		public async Task<bool> DeleteComment(Guid id)
		{
			if (!await this._postRepository.DoesCommentExist(id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = await this._postRepository.GetCommentByIdAsync(id);
			bool result = await this._postRepository.DeleteCommentAsync(comment);

			return result;
		}
	}
}