using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Services
{
	public class LanguageService : ILanguageService
	{
		private readonly ILanguageRepository _languageRepository;
		private readonly IMapper _languageMapper;

		public LanguageService(ILanguageRepository languageRepository, IMapper mapper)
		{
			this._languageRepository = languageRepository;
			this._languageMapper = mapper;
		}

		#region Create

		public async Task<Guid> CreateLanguage(CreateLanguageServiceModel createLanguageServiceModel)
		{
			if (await this._languageRepository.DoesLanguageNameExistAsync(createLanguageServiceModel.Name))
				throw new ArgumentException("Language already exists!");

			Language language = this._languageMapper.Map<Language>(createLanguageServiceModel);
			bool success = await this._languageRepository.AddAsync(language);

			if(success)
			{
				Language newLanguage = await this._languageRepository.GetByNameAsync(createLanguageServiceModel.Name);
				return newLanguage.Id;
			}
			else
				return Guid.Empty;
		}
		#endregion

		#region Read

		public async Task<ReadLanguageServiceModel> GetLanguageById(Guid id)
		{
			Language language = await this._languageRepository.GetByIdAsync(id);

			if (language == null)
				throw new ArgumentException("The language does not exist");

			return this._languageMapper.Map<ReadLanguageServiceModel>(language);
		}
		#endregion

		#region Update

		public async Task<bool> UpdateLanguage(UpdateLanguageServiceModel languageServiceModel)
		{
			bool langExists = await this._languageRepository.DoesLanguageExistAsync(languageServiceModel.Id);
			bool newLangNameExists = await this._languageRepository.DoesLanguageNameExistAsync(languageServiceModel.Name);

			if (!langExists)
				throw new ArgumentException("Language does not exist!");

			if (newLangNameExists)
				throw new ArgumentException("This name is already in our datbase!");

			Language lang = this._languageMapper.Map<Language>(languageServiceModel);
			return await this._languageRepository.EditAsync(lang);
		}
		#endregion

		#region Delete

		public async Task<bool> DeleteLanguage(Guid id)
		{
			if (!await this._languageRepository.DoesLanguageExistAsync(id))
				throw new ArgumentException("Language does not exist!");

			Language language = await this._languageRepository.GetByIdAsync(id);
			return await this._languageRepository.DeleteAsync(language);
		}
		#endregion
	}
}
