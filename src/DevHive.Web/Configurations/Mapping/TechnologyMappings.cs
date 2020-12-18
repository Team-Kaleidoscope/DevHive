using AutoMapper;
using DevHive.Web.Models.Technology;
using DevHive.Services.Models.Technology;

namespace DevHive.Web.Configurations.Mapping
{
    public class TechnologyMappings : Profile
	{
		public TechnologyMappings()
		{
			CreateMap<TechnologyWebModel, TechnologyServiceModel>();
			CreateMap<UpdateTechnologyWebModel, UpdateTechnologyServiceModel>();
			CreateMap<TechnologyServiceModel, TechnologyWebModel>();
			CreateMap<TechnologyWebModel, TechnologyServiceModel>();
		}
	} 
}