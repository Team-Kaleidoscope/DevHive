using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Technology;

namespace DevHive.Services.Services
{
	public class TechnologyService
	{
		private readonly TechnologyRepository _technologyRepository;
		private readonly IMapper _technologyMapper;

		public TechnologyService(TechnologyRepository technologyRepository, IMapper technologyMapper)
		{
			this._technologyRepository = technologyRepository;
			this._technologyMapper = technologyMapper;
		}
	
		public async Task<bool> Create(TechnologyServiceModel technologyServiceModel)
		{
			if (await this._technologyRepository.DoesTechnologyNameExist(technologyServiceModel.Name))
				throw new ArgumentException("Technology already exists!");

			Technology technology = this._technologyMapper.Map<Technology>(technologyServiceModel);
			bool result = await this._technologyRepository.AddAsync(technology);

			return result;
		}
	
		public async Task<TechnologyServiceModel> GetTechnologyById(Guid id)
		{
			Technology technology = await this._technologyRepository.GetByIdAsync(id);

			if(technology == null)
				throw new ArgumentException("The technology does not exist");

			return this._technologyMapper.Map<TechnologyServiceModel>(technology);
		}

		public async Task<bool> UpdateTechnology(UpdateTechnologyServiceModel updateTechnologyServiceModel)
		{
			if (!await this._technologyRepository.DoesTechnologyExist(updateTechnologyServiceModel.Id))
				throw new ArgumentException("Technology does not exist!");

			if (await this._technologyRepository.DoesTechnologyNameExist(updateTechnologyServiceModel.Name))
				throw new ArgumentException("Technology name already exists!");

			Technology technology = this._technologyMapper.Map<Technology>(updateTechnologyServiceModel);
			bool result = await this._technologyRepository.EditAsync(technology);

			return result;
		}
	
		public async Task<bool> DeleteTechnology(Guid id)
		{
			if (!await this._technologyRepository.DoesTechnologyExist(id))
				throw new ArgumentException("Technology does not exist!");

			Technology technology = await this._technologyRepository.GetByIdAsync(id);
			bool result = await this._technologyRepository.DeleteAsync(technology);

			return result;
		}
	}
}