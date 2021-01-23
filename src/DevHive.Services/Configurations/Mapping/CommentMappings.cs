using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Post.Comment;

namespace DevHive.Services.Configurations.Mapping
{
	public class CommentMappings : Profile
	{
		public CommentMappings()
		{
			CreateMap<CreateCommentServiceModel, Comment>();
			CreateMap<UpdateCommentServiceModel, Comment>()
				.ForMember(dest => dest.Id, src => src.MapFrom(p => p.CommentId))
				.ForMember(dest => dest.Message, src => src.MapFrom(p => p.NewMessage));

			CreateMap<Comment, ReadCommentServiceModel>()
				.ForMember(dest => dest.CommentId, src => src.MapFrom(p => p.Id))
				.ForMember(dest => dest.IssuerFirstName, src => src.Ignore())
				.ForMember(dest => dest.IssuerLastName, src => src.Ignore())
				.ForMember(dest => dest.IssuerUsername, src => src.Ignore());
		}
	}
}
