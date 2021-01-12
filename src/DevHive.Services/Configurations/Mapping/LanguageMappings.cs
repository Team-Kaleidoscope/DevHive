using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Configurations.Mapping
{
	public class LanguageMappings : Profile
	{
		public LanguageMappings()
		{
			CreateMap<LanguageServiceModel, Language>();
			CreateMap<CreateLanguageServiceModel, Language>();
			CreateMap<UpdateLanguageServiceModel, Language>();

			CreateMap<Language, LanguageServiceModel>();
			CreateMap<Language, CreateLanguageServiceModel>();
			CreateMap<Language, UpdateLanguageServiceModel>();
		}
	}
}