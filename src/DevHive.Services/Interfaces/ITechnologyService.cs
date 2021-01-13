using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Interfaces
{
	public interface ITechnologyService
	{
		Task<bool> Create(CreateTechnologyServiceModel technologyServiceModel);

		Task<TechnologyServiceModel> GetTechnologyById(Guid id);

		Task<bool> UpdateTechnology(UpdateTechnologyServiceModel updateTechnologyServiceModel);
		
		Task<bool> DeleteTechnology(Guid id);
	}
}