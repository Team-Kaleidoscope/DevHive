using AutoMapper;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Models.Technology;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class TechnologyServicesTests
	{
		private Mock<ITechnologyRepository> _technologyRepositoryMock;
		private Mock<IMapper> _mapperMock;
		private TechnologyService _technologyService;

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this._technologyRepositoryMock = new Mock<ITechnologyRepository>();
			this._mapperMock = new Mock<IMapper>();
			this._technologyService = new TechnologyService(this._technologyRepositoryMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region CreateTechnology
		[Test]
		public async Task CreateTechnology_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			string technologyName = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			CreateTechnologyServiceModel createTechnologyServiceModel = new()
			{
				Name = technologyName
			};
			Technology technology = new()
			{
				Name = technologyName,
				Id = id
			};

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this._technologyRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Technology>())).Returns(Task.FromResult(true));
			this._technologyRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(technology));
			this._mapperMock.Setup(p => p.Map<Technology>(It.IsAny<CreateTechnologyServiceModel>())).Returns(technology);

			Guid result = await this._technologyService.CreateTechnology(createTechnologyServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task CreateTechnology_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			string technologyName = "Gosho Trapov";

			CreateTechnologyServiceModel createTechnologyServiceModel = new()
			{
				Name = technologyName
			};
			Technology technology = new()
			{
				Name = technologyName
			};

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this._technologyRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Technology>())).Returns(Task.FromResult(false));
			this._mapperMock.Setup(p => p.Map<Technology>(It.IsAny<CreateTechnologyServiceModel>())).Returns(technology);

			Guid result = await this._technologyService.CreateTechnology(createTechnologyServiceModel);

			Assert.IsTrue(result == Guid.Empty);
		}

		[Test]
		public void CreateTechnology_ThrowsArgumentException_WhenEntityAlreadyExists()
		{
			string exceptionMessage = "Technology already exists!";
			string technologyName = "Gosho Trapov";

			CreateTechnologyServiceModel createTechnologyServiceModel = new()
			{
				Name = technologyName
			};

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._technologyService.CreateTechnology(createTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetTechnologyById
		[Test]
		public async Task GetTechnologyById_ReturnsTheTechnology_WhenItExists()
		{
			Guid id = Guid.NewGuid();
			string name = "Gosho Trapov";
			Technology technology = new()
			{
				Name = name
			};
			ReadTechnologyServiceModel readTechnologyServiceModel = new()
			{
				Name = name
			};

			this._technologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
			this._mapperMock.Setup(p => p.Map<ReadTechnologyServiceModel>(It.IsAny<Technology>())).Returns(readTechnologyServiceModel);

			ReadTechnologyServiceModel result = await this._technologyService.GetTechnologyById(id);

			Assert.AreEqual(name, result.Name);
		}

		[Test]
		public void GetTechnologyById_ThrowsException_WhenTechnologyDoesNotExist()
		{
			string exceptionMessage = "The technology does not exist";
			Guid id = Guid.NewGuid();
			this._technologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Technology>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._technologyService.GetTechnologyById(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetTechnologies
		[Test]
		public void GetTechnologies_ReturnsAllLanguages_IfAnyExist()
		{
			ReadTechnologyServiceModel firstTechnology = new ReadTechnologyServiceModel();
			ReadTechnologyServiceModel secondTechnology = new ReadTechnologyServiceModel();
			HashSet<ReadTechnologyServiceModel> technologies = new HashSet<ReadTechnologyServiceModel>();
			technologies.Add(firstTechnology);
			technologies.Add(secondTechnology);

			this._technologyRepositoryMock.Setup(p => p.GetTechnologies()).Returns(new HashSet<Technology>());
			this._mapperMock.Setup(p => p.Map<HashSet<ReadTechnologyServiceModel>>(It.IsAny<HashSet<Technology>>())).Returns(technologies);

			HashSet<ReadTechnologyServiceModel> result = this._technologyService.GetTechnologies();

			Assert.GreaterOrEqual(2, result.Count, "GetTechnologies does not return all technologies");
		}

		[Test]
		public void GetLanguages_ReturnsEmptyHashSet_IfNoLanguagesExist()
		{
			this._technologyRepositoryMock.Setup(p => p.GetTechnologies()).Returns(new HashSet<Technology>());
			this._mapperMock.Setup(p => p.Map<HashSet<ReadTechnologyServiceModel>>(It.IsAny<HashSet<Technology>>())).Returns(new HashSet<ReadTechnologyServiceModel>());

			HashSet<ReadTechnologyServiceModel> result = this._technologyService.GetTechnologies();

			Assert.IsEmpty(result, "GetTechnologies does not return empty string when no technologies exist");
		}
		#endregion

		#region UpdateTechnology
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task UpdateTechnology_ReturnsIfUpdateIsSuccessfull_WhenTechnologyExistsy(bool shouldPass)
		{
			string name = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			Technology technology = new Technology
			{
				Name = name,
				Id = id
			};
			UpdateTechnologyServiceModel updatetechnologyServiceModel = new UpdateTechnologyServiceModel
			{
				Name = name,
			};

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this._technologyRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));
			this._mapperMock.Setup(p => p.Map<Technology>(It.IsAny<UpdateTechnologyServiceModel>())).Returns(technology);

			bool result = await this._technologyService.UpdateTechnology(updatetechnologyServiceModel);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void UpdateTechnology_ThrowsException_WhenTechnologyDoesNotExist()
		{
			string exceptionMessage = "Technology does not exist!";
			UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
			{
			};

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._technologyService.UpdateTechnology(updateTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}

		[Test]
		public void UpdateTechnology_ThrowsException_WhenTechnologyNameAlreadyExists()
		{
			string exceptionMessage = "Technology name already exists!";
			UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
			{
			};

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._technologyService.UpdateTechnology(updateTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteTechnology

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteTechnology_ShouldReturnIfDeletionIsSuccessfull_WhenTechnologyExists(bool shouldPass)
		{
			Guid id = Guid.NewGuid();
			Technology technology = new Technology();

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this._technologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
			this._technologyRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));

			bool result = await this._technologyService.DeleteTechnology(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteTechnology_ThrowsException_WhenTechnologyDoesNotExist()
		{
			string exceptionMessage = "Technology does not exist!";
			Guid id = Guid.NewGuid();

			this._technologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this._technologyService.DeleteTechnology(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
