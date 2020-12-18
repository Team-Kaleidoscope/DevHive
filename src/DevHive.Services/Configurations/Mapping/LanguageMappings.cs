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
			CreateMap<Language, LanguageServiceModel>();
			CreateMap<LanguageServiceModel, UpdateLanguageServiceModel>();
		}
	}
}