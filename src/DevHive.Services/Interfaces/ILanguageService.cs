using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Interfaces
{
	public interface ILanguageService
	{
		Task<Guid> CreateLanguage(CreateLanguageServiceModel createLanguageServiceModel);

		Task<ReadLanguageServiceModel> GetLanguageById(Guid id);
		HashSet<ReadLanguageServiceModel> GetLanguages();

		Task<bool> UpdateLanguage(UpdateLanguageServiceModel languageServiceModel);

		Task<bool> DeleteLanguage(Guid id);
	}
}
