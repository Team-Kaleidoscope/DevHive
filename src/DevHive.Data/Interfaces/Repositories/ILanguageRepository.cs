using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Interfaces;

namespace DevHive.Data.Interfaces.Repositories
{
	public interface ILanguageRepository : IRepository<Language>
	{
		HashSet<Language> GetLanguages();
		Task<Language> GetByNameAsync(string name);

		Task<bool> DoesLanguageExistAsync(Guid id);
		Task<bool> DoesLanguageNameExistAsync(string languageName);
	}
}
