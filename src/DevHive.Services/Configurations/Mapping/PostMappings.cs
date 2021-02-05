using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Post;
using DevHive.Common.Models.Misc;
using System.Collections.Generic;
using DevHive.Services.Models;

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
				.ForMember(dest => dest.CreatorFirstName, src => src.MapFrom(p => p.Creator.FirstName))
				.ForMember(dest => dest.CreatorLastName, src => src.MapFrom(p => p.Creator.LastName))
				.ForMember(dest => dest.CreatorUsername, src => src.MapFrom(p => p.Creator.UserName));

			CreateMap<Post, IdModel>()
				.ForMember(dest => dest.Id, src => src.MapFrom(x => x.Id));

			CreateMap<List<Post>, ReadPageServiceModel>();
		}
	}
}
