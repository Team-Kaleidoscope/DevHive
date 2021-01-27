using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Interfaces
{
	public interface ITechnologyService
	{
		Task<Guid> CreateTechnology(CreateTechnologyServiceModel technologyServiceModel);

		Task<CreateTechnologyServiceModel> GetTechnologyById(Guid id);
		HashSet<ReadTechnologyServiceModel> GetTechnologies();

		Task<bool> UpdateTechnology(UpdateTechnologyServiceModel updateTechnologyServiceModel);

		Task<bool> DeleteTechnology(Guid id);
	}
}
