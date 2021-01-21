using AutoMapper;
using DevHive.Web.Models.Language;
using DevHive.Services.Models.Language;

namespace DevHive.Web.Configurations.Mapping
{
	public class LanguageMappings : Profile
	{
		public LanguageMappings()
		{
			CreateMap<CreateLanguageWebModel, CreateLanguageServiceModel>();
			CreateMap<ReadLanguageWebModel, ReadLanguageServiceModel>();
			CreateMap<UpdateLanguageWebModel, UpdateLanguageServiceModel>()
				.ForMember(src => src.Id, dest => dest.Ignore());
			CreateMap<LanguageWebModel, LanguageServiceModel>();

			CreateMap<LanguageServiceModel, LanguageWebModel>();
			CreateMap<ReadLanguageServiceModel, ReadLanguageWebModel>();
			CreateMap<CreateLanguageServiceModel, CreateLanguageWebModel>();
			CreateMap<UpdateLanguageServiceModel, UpdateLanguageWebModel>();
		}
	}
}
