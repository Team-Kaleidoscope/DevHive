using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Models.Language;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class LanguageServiceTests
	{
		private Mock<ILanguageRepository> _languageRepositoryMock;
		private Mock<IMapper> _mapperMock;
		private LanguageService _languageService;

		#region SetUps
		[SetUp]
		public void SetUp()
		{
			this._languageRepositoryMock = new Mock<ILanguageRepository>();
			this._mapperMock = new Mock<IMapper>();
			this._languageService = new LanguageService(this._languageRepositoryMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region CreateLanguage
		[Test]
		public async Task CreateLanguage_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			string technologyName = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			CreateLanguageServiceModel createLanguageServiceModel = new CreateLanguageServiceModel
			{
				Name = technologyName
			};
			Language language = new Language
			{
				Name = technologyName,
				Id = id
			};

			this._languageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this._languageRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Language>())).Returns(Task.FromResult(true));
			this._languageRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(language));
			this._mapperMock.Setup(p => p.Map<Language>(It.IsAny<CreateLanguageServiceModel>())).Returns(language);

			Guid result = await this._languageService.CreateLanguage(createLanguageServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task CreateLanguage_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			string languageName = "Gosho Trapov";

			CreateLanguageServiceModel createLanguageServiceModel = new CreateLanguageServiceModel
			{
				Name = languageName
			};
			Language language = new Language
			{
				Name = languageName
			};

			this._languageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this._languageRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Language>())).Returns(Task.FromResult(false));
			this._mapperMock.Setup(p => p.Map<Language>(It.IsAny<CreateLanguageServiceModel>())).Returns(language);

			Guid result = await this._languageService.CreateLanguage(createLanguageServiceModel);

			Assert.IsTrue(result == Guid.Empty);

		}

		[Test]
		public void CreateLanguage_ThrowsArgumentException_WhenEntityAlreadyExists()
		{
			string exceptionMessage = "Language already exists!";
			string languageName = "Gosho Trapov";

			CreateLanguageServiceModel createLanguageServiceModel = new CreateLanguageServiceModel
			{
				Name = languageName
			};
			Language language = new Language
			{
				Name = languageName
			};

			this._languageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._languageService.CreateLanguage(createLanguageServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetLanguageById
		[Test]
		public async Task GetLanguageById_ReturnsTheLanguage_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			string name = "Gosho Trapov";
			Language language = new Language
			{
				Name = name
			};
			ReadLanguageServiceModel readLanguageServiceModel = new ReadLanguageServiceModel
			{
				Name = name
			};

			this._languageRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(language));
			this._mapperMock.Setup(p => p.Map<ReadLanguageServiceModel>(It.IsAny<Language>())).Returns(readLanguageServiceModel);

			ReadLanguageServiceModel result = await this._languageService.GetLanguageById(id);

			Assert.AreEqual(name, result.Name);
		}

		[Test]
		public void GetLanguageById_ThrowsException_WhenLanguageDoesNotExist()
		{
			string exceptionMessage = "The language does not exist";
			Guid id = Guid.NewGuid();
			this._languageRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Language>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._languageService.GetLanguageById(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetLanguages
		[Test]
		public void GetLanguages_ReturnsAllLanguages_IfAnyExist()
		{
			ReadLanguageServiceModel firstLanguage = new ReadLanguageServiceModel();
			ReadLanguageServiceModel secondLanguage = new ReadLanguageServiceModel();
			HashSet<ReadLanguageServiceModel> languges = new HashSet<ReadLanguageServiceModel>();
			languges.Add(firstLanguage);
			languges.Add(secondLanguage);

			this._languageRepositoryMock.Setup(p => p.GetLanguages()).Returns(new HashSet<Language>());
			this._mapperMock.Setup(p => p.Map<HashSet<ReadLanguageServiceModel>>(It.IsAny<HashSet<Language>>())).Returns(languges);

			HashSet<ReadLanguageServiceModel> result = this._languageService.GetLanguages();

			Assert.GreaterOrEqual(2, result.Count, "GetLanguages does not return all languages");
		}

		[Test]
		public void GetLanguages_ReturnsEmptyHashSet_IfNoLanguagesExist()
		{
			this._languageRepositoryMock.Setup(p => p.GetLanguages()).Returns(new HashSet<Language>());
			this._mapperMock.Setup(p => p.Map<HashSet<ReadLanguageServiceModel>>(It.IsAny<HashSet<Language>>())).Returns(new HashSet<ReadLanguageServiceModel>());

			HashSet<ReadLanguageServiceModel> result = this._languageService.GetLanguages();

			Assert.IsEmpty(result, "GetLanguages does not return empty string when no languages exist");
		}
		#endregion

		#region UpdateLanguage
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task UpdateLanguage_ReturnsIfUpdateIsSuccessfull_WhenLanguageExistsy(bool shouldPass)
		{
			string name = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			Language language = new Language
			{
				Name = name,
				Id = id
			};
			UpdateLanguageServiceModel updateLanguageServiceModel = new UpdateLanguageServiceModel
			{
				Name = name,
			};

			this._languageRepositoryMock.Setup(p => p.DoesLanguageExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this._languageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this._languageRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Language>())).Returns(Task.FromResult(shouldPass));
			this._mapperMock.Setup(p => p.Map<Language>(It.IsAny<UpdateLanguageServiceModel>())).Returns(language);

			bool result = await this._languageService.UpdateLanguage(updateLanguageServiceModel);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void UpdateLanguage_ThrowsArgumentException_WhenLanguageDoesNotExist()
		{
			string exceptionMessage = "Language does not exist!";
			UpdateLanguageServiceModel updateTechnologyServiceModel = new UpdateLanguageServiceModel
			{
			};

			this._languageRepositoryMock.Setup(p => p.DoesLanguageExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._languageService.UpdateLanguage(updateTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}

		[Test]
		public void UpdateLanguage_ThrowsException_WhenLanguageNameAlreadyExists()
		{
			string exceptionMessage = "Language name already exists in our data base!";
			UpdateLanguageServiceModel updateTechnologyServiceModel = new UpdateLanguageServiceModel
			{
			};

			this._languageRepositoryMock.Setup(p => p.DoesLanguageExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this._languageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._languageService.UpdateLanguage(updateTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteLanguage
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteLanguage_ShouldReturnIfDeletionIsSuccessfull_WhenLanguageExists(bool shouldPass)
		{
			Guid id = Guid.NewGuid();
			Language language = new Language();

			this._languageRepositoryMock.Setup(p => p.DoesLanguageExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this._languageRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(language));
			this._languageRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Language>())).Returns(Task.FromResult(shouldPass));

			bool result = await this._languageService.DeleteLanguage(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteLanguage_ThrowsException_WhenLanguageDoesNotExist()
		{
			string exceptionMessage = "Language does not exist!";
			Guid id = Guid.NewGuid();

			this._languageRepositoryMock.Setup(p => p.DoesLanguageExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._languageService.DeleteLanguage(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
