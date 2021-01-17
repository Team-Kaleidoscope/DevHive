using System;
using System.Threading.Tasks;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Interfaces
{
	public interface ILanguageService
	{
		Task<bool> CreateLanguage(CreateLanguageServiceModel createLanguageServiceModel);

		Task<ReadLanguageServiceModel> GetLanguageById(Guid id);

		Task<bool> UpdateLanguage(UpdateLanguageServiceModel languageServiceModel);

		Task<bool> DeleteLanguage(Guid id);
	}
}
