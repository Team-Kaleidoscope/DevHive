using AutoMapper;
using DevHive.Services.Models.Post.Comment;
using DevHive.Web.Models.Post.Comment;

namespace DevHive.Web.Configurations.Mapping
{
	public class CommentMappings : Profile
	{
		public CommentMappings()
		{
			CreateMap<CreateCommentWebModel, CreateCommentServiceModel>();
			CreateMap<UpdateCommentWebModel, UpdateCommentServiceModel>();

			CreateMap<ReadCommentServiceModel, ReadCommentWebModel>()
				.ForMember(dest => dest.IssuerFirstName, src => src.Ignore())
				.ForMember(dest => dest.IssuerLastName, src => src.Ignore())
				.ForMember(dest => dest.IssuerUsername, src => src.Ignore());
		}
	}
}



