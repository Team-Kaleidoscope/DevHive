using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Comment;

namespace DevHive.Services.Configurations.Mapping
{
	public class CommentMappings : Profile
	{
		public CommentMappings()
		{
			CreateMap<CommentServiceModel, Comment>();
			CreateMap<Language, CommentServiceModel>();
			CreateMap<UpdateCommentServiceModel, Language>();
			CreateMap<GetByIdCommentServiceModel, Language>();
		}
	}
}