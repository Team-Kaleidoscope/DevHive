using AutoMapper;
using DevHive.Services.Models.Post.Comment;
using DevHive.Web.Models.Post.Comment;

namespace DevHive.Web.Configurations.Mapping
{
	public class CommentMappings : Profile
	{
		public CommentMappings()
		{
			CreateMap<CommentWebModel, CommentServiceModel>();
			CreateMap<CommentWebModel, UpdateCommentServiceModel>();
			CreateMap<CommentServiceModel, CommentWebModel>();
			CreateMap<CommentWebModel, CommentServiceModel>();
		}
	} 
}