using AutoMapper;
using DevHive.Web.Models.Technology;
using DevHive.Services.Models.Technology;

namespace DevHive.Web.Configurations.Mapping
{
	public class TechnologyMappings : Profile
	{
		public TechnologyMappings()
		{
			CreateMap<CreateTechnologyWebModel, CreateTechnologyServiceModel>();
			CreateMap<ReadTechnologyWebModel, ReadTechnologyServiceModel>();
			CreateMap<UpdateTechnologyWebModel, UpdateTechnologyServiceModel>()
				.ForMember(src => src.Id, dest => dest.Ignore());
			CreateMap<TechnologyWebModel, TechnologyServiceModel>();

			CreateMap<CreateTechnologyServiceModel, CreateTechnologyWebModel>();
			CreateMap<ReadTechnologyServiceModel, ReadTechnologyWebModel>();
			CreateMap<UpdateTechnologyServiceModel, UpdateTechnologyWebModel>();
			CreateMap<TechnologyServiceModel, TechnologyWebModel>();
		}
	}
}
