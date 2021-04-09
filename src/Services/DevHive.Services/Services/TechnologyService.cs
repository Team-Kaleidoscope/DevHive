using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Constants;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Services
{
	public class TechnologyService : ITechnologyService
	{
		private readonly ITechnologyRepository _technologyRepository;
		private readonly IMapper _technologyMapper;

		public TechnologyService(ITechnologyRepository technologyRepository, IMapper technologyMapper)
		{
			this._technologyRepository = technologyRepository;
			this._technologyMapper = technologyMapper;
		}

		#region Create
		public async Task<Guid> CreateTechnology(CreateTechnologyServiceModel technologyServiceModel)
		{
			if (await this._technologyRepository.DoesTechnologyNameExistAsync(technologyServiceModel.Name))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Technology));

			Technology technology = this._technologyMapper.Map<Technology>(technologyServiceModel);
			bool success = await this._technologyRepository.AddAsync(technology);

			if (success)
			{
				Technology newTechnology = await this._technologyRepository.GetByNameAsync(technologyServiceModel.Name);
				return newTechnology.Id;
			}
			else
				return Guid.Empty;
		}
		#endregion

		#region Read
		public async Task<ReadTechnologyServiceModel> GetTechnologyById(Guid id)
		{
			Technology technology = await this._technologyRepository.GetByIdAsync(id);

			if (technology == null)
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Technology));

			return this._technologyMapper.Map<ReadTechnologyServiceModel>(technology);
		}

		public HashSet<ReadTechnologyServiceModel> GetTechnologies()
		{
			HashSet<Technology> technologies = this._technologyRepository.GetTechnologies();

			return this._technologyMapper.Map<HashSet<ReadTechnologyServiceModel>>(technologies);
		}
		#endregion

		#region Update
		public async Task<bool> UpdateTechnology(UpdateTechnologyServiceModel updateTechnologyServiceModel)
		{
			if (!await this._technologyRepository.DoesTechnologyExistAsync(updateTechnologyServiceModel.Id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Technology));

			if (await this._technologyRepository.DoesTechnologyNameExistAsync(updateTechnologyServiceModel.Name))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Technology));

			Technology technology = this._technologyMapper.Map<Technology>(updateTechnologyServiceModel);
			bool result = await this._technologyRepository.EditAsync(updateTechnologyServiceModel.Id, technology);

			return result;
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteTechnology(Guid id)
		{
			if (!await this._technologyRepository.DoesTechnologyExistAsync(id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Technology));

			Technology technology = await this._technologyRepository.GetByIdAsync(id);
			bool result = await this._technologyRepository.DeleteAsync(technology);

			return result;
		}
		#endregion
	}
}
