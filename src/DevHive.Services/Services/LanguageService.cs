using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Contracts;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Services
{
	public class LanguageService
	{
		private readonly ILanguageRepository _languageRepository;
		private readonly IMapper _languageMapper;

		public LanguageService(ILanguageRepository languageRepository, IMapper mapper)
		{
			this._languageRepository = languageRepository;
			this._languageMapper = mapper;
		}

		public async Task<bool> CreateLanguage(LanguageServiceModel languageServiceModel)
		{
			if (await this._languageRepository.DoesLanguageNameExist(languageServiceModel.Name))
				throw new ArgumentException("Language already exists!");

			Language language = this._languageMapper.Map<Language>(languageServiceModel);
			bool result = await this._languageRepository.AddAsync(language);

			return result;
		}

		public async Task<LanguageServiceModel> GetLanguageById(Guid id)
		{
			Language language = await this._languageRepository.GetByIdAsync(id);

			if(language == null)
				throw new ArgumentException("The language does not exist");

			return this._languageMapper.Map<LanguageServiceModel>(language);
		}

		public async Task<bool> UpdateLanguage(UpdateLanguageServiceModel languageServiceModel)
		{
			if (!await this._languageRepository.DoesLanguageExist(languageServiceModel.Id))
				throw new ArgumentException("Language does not exist!");

			if (await this._languageRepository.DoesLanguageNameExist(languageServiceModel.Name))
				throw new ArgumentException("Language name already exists!");

			Language language = this._languageMapper.Map<Language>(languageServiceModel);
			//language.Id = languageServiceModel.Id;
			bool result = await this._languageRepository.EditAsync(language);

			return result;
		}
	
		public async Task<bool> DeleteLanguage(Guid id)
		{
			if (!await this._languageRepository.DoesLanguageExist(id))
				throw new ArgumentException("Language does not exist!");

			Language language = await this._languageRepository.GetByIdAsync(id);
			bool result = await this._languageRepository.DeleteAsync(language);

			return result;
		}
	}
}