using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Comment;

namespace DevHive.Services.Services
{
	public class CommentService
	{
		private readonly CommentRepository _commentRepository;
		private readonly IMapper _commentMapper;

		public CommentService(CommentRepository commentRepository, IMapper mapper)
		{
			this._commentRepository = commentRepository;
			this._commentMapper = mapper;
		}

		public async Task<bool> CreateComment(CommentServiceModel commentServiceModel)
		{
			Comment comment = this._commentMapper.Map<Comment>(commentServiceModel);
			comment.Date = DateTime.Now;
			bool result = await this._commentRepository.AddAsync(comment);

			return result;
		}
	
		public async Task<GetByIdCommentServiceModel> GetCommentById(Guid id)
		{
			Comment comment = await this._commentRepository.GetByIdAsync(id);

			if(comment == null)
				throw new ArgumentException("The comment does not exist");

			return this._commentMapper.Map<GetByIdCommentServiceModel>(comment);
		}

		public async Task<bool> UpdateComment(UpdateCommentServiceModel commentServiceModel)
		{
			if (!await this._commentRepository.DoesCommentExist(commentServiceModel.Id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = this._commentMapper.Map<Comment>(commentServiceModel);
			bool result = await this._commentRepository.EditAsync(comment);

			return result;
		}
	
		public async Task<bool> DeleteComment(Guid id)
		{
			if (!await this._commentRepository.DoesCommentExist(id))
				throw new ArgumentException("Comment does not exist!");

			Comment comment = await this._commentRepository.GetByIdAsync(id);
			bool result = await this._commentRepository.DeleteAsync(comment);

			return result;
		}
	}
}