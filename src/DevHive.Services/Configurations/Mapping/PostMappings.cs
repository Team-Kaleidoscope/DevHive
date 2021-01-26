using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Post.Post;

namespace DevHive.Services.Configurations.Mapping
{
	public class PostMappings : Profile
	{
		public PostMappings()
		{
			CreateMap<CreatePostServiceModel, Post>();
				// .ForMember(dest => dest.Files, src => src.Ignore());
			CreateMap<UpdatePostServiceModel, Post>()
				.ForMember(dest => dest.Id, src => src.MapFrom(p => p.PostId))
				// .ForMember(dest => dest.Files, src => src.Ignore())
				.ForMember(dest => dest.Message, src => src.MapFrom(p => p.NewMessage));

			CreateMap<Post, ReadPostServiceModel>()
				.ForMember(dest => dest.PostId, src => src.MapFrom(p => p.Id))
				.ForMember(dest => dest.CreatorFirstName, src => src.Ignore())
				.ForMember(dest => dest.CreatorLastName, src => src.Ignore())
				.ForMember(dest => dest.CreatorUsername, src => src.Ignore());
				//TODO: Map those here /\
		}
	}
}
