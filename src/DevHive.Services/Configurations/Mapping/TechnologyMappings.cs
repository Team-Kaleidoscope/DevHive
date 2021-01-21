using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Configurations.Mapping
{
	public class TechnologyMappings : Profile
	{
		public TechnologyMappings()
		{
			CreateMap<CreateTechnologyServiceModel, Technology>();
			CreateMap<UpdateTechnologyServiceModel, Technology>();
			CreateMap<TechnologyServiceModel, Technology>();

			CreateMap<Technology, CreateTechnologyServiceModel>();
			CreateMap<Technology, TechnologyServiceModel>();
			CreateMap<Technology, UpdateTechnologyServiceModel>();
		}
	}
}
