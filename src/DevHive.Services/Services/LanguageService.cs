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

		public async Task<IActionResult> CreateLanguage(LanguageServiceModel languageServiceModel)
		{
			if (!await this._languageRepository.DoesLanguageNameExist(languageServiceModel.Name))
				return new BadRequestObjectResult("Language already exists!");

			Language language = this._languageMapper.Map<Language>(languageServiceModel);

			await this._languageRepository.AddAsync(language);

			return new CreatedResult("CreateLanguage", language);
		}

		public async Task<IActionResult> GetLanguageByID(Guid id)
		{
			Language language = await this._languageRepository.GetByIdAsync(id);

			if(language == null)
				return new NotFoundObjectResult("The language does not exist");

			return new ObjectResult(language);
		}

		public async Task<IActionResult> UpdateLanguage(LanguageServiceModel languageServiceModel)
		{
			if (!await this._languageRepository.DoesLanguageExist(languageServiceModel.Id))
				return new NotFoundObjectResult("Language does not exist!");

			if (!await this._languageRepository.DoesLanguageNameExist(languageServiceModel.Name))
				return new BadRequestObjectResult("Language name already exists!");

			Language language = this._languageMapper.Map<Language>(languageServiceModel);
			await this._languageRepository.EditAsync(language);

			return new AcceptedResult("UpdateLanguage", language);
		}
	
		public async Task<IActionResult> DeleteLanguage(Guid id)
		{
			if (!await this._languageRepository.DoesLanguageExist(id))
				return new NotFoundObjectResult("Language does not exist!");

			Language language = await this._languageRepository.GetByIdAsync(id);
			await this._languageRepository.DeleteAsync(language);

			return new OkResult();
		}
	}
}