using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Language;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Services.Services
{
	public class LanguageService
	{
		private readonly LanguageRepository _languageRepository;
		private readonly IMapper _languageMapper;

		public LanguageService(DevHiveContext context, IMapper mapper)
		{
			this._languageRepository = new LanguageRepository(context);
			this._languageMapper = mapper;
		}

		public async Task<LanguageServiceModel> CreateLanguage(LanguageServiceModel languageServiceModel)
		{
			if (!await this._languageRepository.DoesLanguageNameExist(languageServiceModel.Name))
				throw new ArgumentException("Language already exists!");

			Language language = this._languageMapper.Map<Language>(languageServiceModel);

			await this._languageRepository.AddAsync(language);

			return this._languageMapper.Map<LanguageServiceModel>(language);
		}

		public async Task<LanguageServiceModel> GetLanguageById(Guid id)
		{
			Language language = await this._languageRepository.GetByIdAsync(id);

			if(language == null)
				throw new ArgumentException("The language does not exist");

			return this._languageMapper.Map<LanguageServiceModel>(language);
		}

		public async Task<LanguageServiceModel> UpdateLanguage(LanguageServiceModel languageServiceModel)
		{
			if (!await this._languageRepository.DoesLanguageExist(languageServiceModel.Id))
				throw new ArgumentException("Language does not exist!");

			if (!await this._languageRepository.DoesLanguageNameExist(languageServiceModel.Name))
				throw new ArgumentException("Language name already exists!");

			Language language = this._languageMapper.Map<Language>(languageServiceModel);
			await this._languageRepository.EditAsync(language);

			return this._languageMapper.Map<LanguageServiceModel>(language);
		}
	
		public async Task<LanguageServiceModel> DeleteLanguage(Guid id)
		{
			if (!await this._languageRepository.DoesLanguageExist(id))
				throw new ArgumentException("Language does not exist!");

			Language language = await this._languageRepository.GetByIdAsync(id);
			await this._languageRepository.DeleteAsync(language);

			return this._languageMapper.Map<LanguageServiceModel>(language);
		}
	}
}