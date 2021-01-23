using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Post.Comment;

namespace DevHive.Services.Configurations.Mapping
{
	public class CommentMappings : Profile
	{
		public CommentMappings()
		{
			CreateMap<CreateCommentServiceModel, Comment>()
				.ForMember(src => src.Id, dest => dest.Ignore());
			CreateMap<UpdateCommentServiceModel, Comment>()
				.ForMember(src => src.Id, dest => dest.MapFrom(p => p.CommentId));

			CreateMap<Comment, ReadCommentServiceModel>();
			CreateMap<Comment, UpdateCommentServiceModel>()
				.ForMember(src => src.CommentId, dest => dest.MapFrom(p => p.Id));
		}
	}
}
