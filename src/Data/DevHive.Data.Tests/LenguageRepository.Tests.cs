using System;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class LenguageRepositoryTests
	{
		private const string LANGUAGE_NAME = "Language test name";
		protected DevHiveContext Context { get; set; }
		protected LanguageRepository LanguageRepository { get; set; }

		#region Setups
		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this.Context = new DevHiveContext(optionsBuilder.Options);

			LanguageRepository = new LanguageRepository(Context);
		}

		[TearDown]
		public void TearDown()
		{
			this.Context.Database.EnsureDeleted();
		}
		#endregion

		#region GetByNameAsync
		[Test]
		public async Task GetByNameAsync_ReturnsTheCorrectLanguage_IfItExists()
		{
			await AddEntity();

			Language language = this.Context.Languages.Where(x => x.Name == LANGUAGE_NAME).ToList().FirstOrDefault();

			Language languageResult = await this.LanguageRepository.GetByNameAsync(LANGUAGE_NAME);

			Assert.AreEqual(language.Id, languageResult.Id);
		}

		[Test]
		public async Task GetByNameAsync_ReturnsNull_IfTechnologyDoesNotExists()
		{
			Language languageResult = await this.LanguageRepository.GetByNameAsync(LANGUAGE_NAME);

			Assert.IsNull(languageResult);
		}
		#endregion

		#region DoesLanguageExistAsync
		[Test]
		public async Task DoesLanguageExist_ReturnsTrue_IfIdExists()
		{
			await AddEntity();
			Language language = this.Context.Languages.Where(x => x.Name == LANGUAGE_NAME).ToList().FirstOrDefault();

			Guid id = language.Id;

			bool result = await this.LanguageRepository.DoesLanguageExistAsync(id);

			Assert.IsTrue(result, "DoesLanguageExistAsync returns flase when language exists");
		}

		[Test]
		public async Task DoesLanguageExist_ReturnsFalse_IfIdDoesNotExists()
		{
			Guid id = Guid.NewGuid();

			bool result = await this.LanguageRepository.DoesLanguageExistAsync(id);

			Assert.IsFalse(result, "DoesLanguageExistAsync returns true when language does not exist");
		}
		#endregion

		#region DoesTechnologyNameExistAsync
		[Test]
		public async Task DoesLanguageNameExist_ReturnsTrue_IfLanguageExists()
		{
			await AddEntity();

			bool result = await this.LanguageRepository.DoesLanguageNameExistAsync(LANGUAGE_NAME);

			Assert.IsTrue(result, "DoesLanguageNameExists returns true when language name does not exist");
		}

		[Test]
		public async Task DoesLanguageNameExist_ReturnsFalse_IfLanguageDoesNotExists()
		{
			bool result = await this.LanguageRepository.DoesLanguageNameExistAsync(LANGUAGE_NAME);

			Assert.False(result, "DoesTechnologyNameExistAsync returns true when language name does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task AddEntity(string name = LANGUAGE_NAME)
		{
			Language language = new Language
			{
				Name = name
			};

			await this.LanguageRepository.AddAsync(language);
		}
		#endregion
	}
}
