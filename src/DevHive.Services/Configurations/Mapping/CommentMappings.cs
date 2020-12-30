using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Post.Comment;
using DevHive.Common.Models.Misc;

namespace DevHive.Services.Configurations.Mapping
{
	public class CommentMappings : Profile
	{
		public CommentMappings()
		{
			CreateMap<CommentServiceModel, Comment>();
			CreateMap<Comment, CommentServiceModel>();
			CreateMap<UpdateCommentServiceModel, Comment>();
			CreateMap<IdModel, Comment>();
			CreateMap<Comment, IdModel>();
		}
	}
}