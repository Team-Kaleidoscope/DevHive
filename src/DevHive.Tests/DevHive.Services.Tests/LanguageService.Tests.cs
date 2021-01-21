using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
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
		private Mock<ILanguageRepository> LanguageRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private LanguageService LanguageService { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.LanguageRepositoryMock = new Mock<ILanguageRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.LanguageService = new LanguageService(this.LanguageRepositoryMock.Object, this.MapperMock.Object);
		}

		#region Create
		[Test]
		public async void Create_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			string technologyName = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			CreateLanguageServiceModel createLanguageServiceModel = new()
			{
				Name = technologyName
			};
			Language language = new()
			{
				Name = technologyName,
				Id = id
			};

			this.LanguageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.LanguageRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Language>())).Returns(Task.FromResult(true));
			this.LanguageRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(language));
			this.MapperMock.Setup(p => p.Map<Language>(It.IsAny<CreateLanguageServiceModel>())).Returns(language);

			Guid result = await this.LanguageService.CreateLanguage(createLanguageServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async void Create_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			string languageName = "Gosho Trapov";

			CreateLanguageServiceModel createLanguageServiceModel = new()
			{
				Name = languageName
			};
			Language language = new()
			{
				Name = languageName
			};

			this.LanguageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.LanguageRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Language>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Language>(It.IsAny<CreateLanguageServiceModel>())).Returns(language);

			Guid result = await this.LanguageService.CreateLanguage(createLanguageServiceModel);

			Assert.IsTrue(result == Guid.Empty);
		}

		[Test]
		public void Create_ThrowsArgumentException_WhenEntityAlreadyExists()
		{
			string exceptionMessage = "Technology already exists!";
			string languageName = "Gosho Trapov";

			CreateLanguageServiceModel createLanguageServiceModel = new()
			{
				Name = languageName
			};
			Language language = new()
			{
				Name = languageName
			};

			this.LanguageRepositoryMock.Setup(p => p.DoesLanguageNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.LanguageService.CreateLanguage(createLanguageServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
