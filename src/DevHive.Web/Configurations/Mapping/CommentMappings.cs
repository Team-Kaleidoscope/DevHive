using AutoMapper;
using DevHive.Web.Models.Comment;
using DevHive.Services.Models.Comment;

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
			CreateMap<GetByIdCommentServiceModel, GetByIdCommentWebModel>();
		}
	} 
}