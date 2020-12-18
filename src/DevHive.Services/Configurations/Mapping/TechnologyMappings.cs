using DevHive.Data.Models;
using AutoMapper;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Configurations.Mapping
{
	public class TechnologyMappings : Profile
	{
		public TechnologyMappings()
		{
			CreateMap<TechnologyServiceModel, Technology>();
			CreateMap<Technology, TechnologyServiceModel>();
		}
	}
}