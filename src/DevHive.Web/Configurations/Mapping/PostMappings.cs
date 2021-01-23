using AutoMapper;
using DevHive.Services.Models.Post.Post;
using DevHive.Web.Models.Post.Post;

namespace DevHive.Web.Configurations.Mapping
{
	public class PostMappings : Profile
	{
		public PostMappings()
		{
			CreateMap<CreatePostWebModel, CreatePostServiceModel>();
			CreateMap<UpdatePostWebModel, UpdatePostServiceModel>();

			CreateMap<ReadPostServiceModel, ReadPostWebModel>();
		}
	}
}
