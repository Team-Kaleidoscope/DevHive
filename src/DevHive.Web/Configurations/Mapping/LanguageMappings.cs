using AutoMapper;
using DevHive.Web.Models.Language;
using DevHive.Services.Models.Language;

namespace DevHive.Web.Configurations.Mapping
{
    public class LanguageMappings : Profile
	{
		public LanguageMappings()
		{
			CreateMap<LanguageWebModel, LanguageServiceModel>();
			CreateMap<UpdateLanguageWebModel, LanguageServiceModel>();
			CreateMap<LanguageServiceModel, LanguageWebModel>();
			CreateMap<LanguageWebModel, UpdateLanguageServiceModel>();
		}
	} 
}