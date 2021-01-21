using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
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
		public async Task<Guid> Create(CreateTechnologyServiceModel technologyServiceModel)
		{
			if (await this._technologyRepository.DoesTechnologyNameExistAsync(technologyServiceModel.Name))
				throw new ArgumentException("Technology already exists!");

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
		public async Task<CreateTechnologyServiceModel> GetTechnologyById(Guid id)
		{
			Technology technology = await this._technologyRepository.GetByIdAsync(id);

			if (technology == null)
				throw new ArgumentException("The technology does not exist");

			return this._technologyMapper.Map<CreateTechnologyServiceModel>(technology);
		}
		#endregion

		#region Update
		public async Task<bool> UpdateTechnology(UpdateTechnologyServiceModel updateTechnologyServiceModel)
		{
			if (!await this._technologyRepository.DoesTechnologyExistAsync(updateTechnologyServiceModel.Id))
				throw new ArgumentException("Technology does not exist!");

			if (await this._technologyRepository.DoesTechnologyNameExistAsync(updateTechnologyServiceModel.Name))
				throw new ArgumentException("Technology name already exists!");

			Technology technology = this._technologyMapper.Map<Technology>(updateTechnologyServiceModel);
			bool result = await this._technologyRepository.EditAsync(technology);

			return result;
		}
		#endregion

		#region Delete
		public async Task<bool> DeleteTechnology(Guid id)
		{
			if (!await this._technologyRepository.DoesTechnologyExistAsync(id))
				throw new ArgumentException("Technology does not exist!");

			Technology technology = await this._technologyRepository.GetByIdAsync(id);
			bool result = await this._technologyRepository.DeleteAsync(technology);

			return result;
		}
		#endregion
	}
}
