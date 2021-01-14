using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Interfaces
{
	public interface ILanguageService
	{
		Task<bool> CreateLanguage(CreateLanguageServiceModel createLanguageServiceModel);

		Task<LanguageServiceModel> GetLanguageById(Guid languageId);

		Task<bool> UpdateLanguage(Guid languageId, UpdateLanguageServiceModel languageServiceModel);

		Task<bool> DeleteLanguage(Guid languageId);
	}
}
