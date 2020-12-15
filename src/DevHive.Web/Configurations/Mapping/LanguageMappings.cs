using AutoMapper;
using DevHive.Web.Models.Language;
using DevHive.Services.Models.Language;

namespace DevHive.Web.Configurations.Mapping
{
    public class LanguageMappings : Profile
	{
		public LanguageMappings()
		{
			CreateMap<CreateLanguageWebModel, LanguageServiceModel>();
			CreateMap<UpdateLanguageWebModel, LanguageServiceModel>();
		}
	} 
}