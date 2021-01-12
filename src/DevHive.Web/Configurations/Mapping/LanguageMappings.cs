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
			CreateMap<CreateLanguageWebModel, CreateLanguageServiceModel>();
			CreateMap<UpdateLanguageWebModel, UpdateLanguageServiceModel>();

			CreateMap<LanguageServiceModel, LanguageWebModel>();
			CreateMap<CreateLanguageServiceModel, CreateLanguageWebModel>();
			CreateMap<UpdateLanguageServiceModel, UpdateLanguageWebModel>();
		}
	} 
}