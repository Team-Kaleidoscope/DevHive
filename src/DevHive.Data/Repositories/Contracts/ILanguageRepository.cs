using DevHive.Data.Models;
using System;
using System.Threading.Tasks;

namespace DevHive.Data.Repositories.Contracts
{
    public interface ILanguageRepository : IRepository<Language>
    {
		public Task<bool> DoesLanguageNameExist(string languageName);

		public Task<bool> DoesLanguageExist(Guid id);
	}
}
