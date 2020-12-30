using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Post;
using DevHive.Services.Models.Post.Post;

namespace DevHive.Services.Configurations.Mapping
{
	public class PostMappings : Profile
	{
		public PostMappings()
		{
			CreateMap<PostServiceModel, Post>();
			CreateMap<Post, PostServiceModel>();
		}
	} 
}
