using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
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
		private Mock<ITechnologyRepository> TechnologyRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private TechnologyService TechnologyService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.TechnologyRepositoryMock = new Mock<ITechnologyRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.TechnologyService = new TechnologyService(this.TechnologyRepositoryMock.Object, this.MapperMock.Object);
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

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.TechnologyRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Technology>())).Returns(Task.FromResult(true));
			this.TechnologyRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(technology));
			this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<CreateTechnologyServiceModel>())).Returns(technology);

			Guid result = await this.TechnologyService.CreateTechnology(createTechnologyServiceModel);

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

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.TechnologyRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Technology>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<CreateTechnologyServiceModel>())).Returns(technology);

			Guid result = await this.TechnologyService.CreateTechnology(createTechnologyServiceModel);

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
			Technology technology = new()
			{
				Name = technologyName
			};

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.TechnologyService.CreateTechnology(createTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetTechnologyById
		[Test]
		public async Task GetTechnologyById_ReturnsTheTechnology_WhenItExists()
		{
			Guid id = new Guid();
			string name = "Gosho Trapov";
			Technology technology = new()
			{
				Name = name
			};
			ReadTechnologyServiceModel readTechnologyServiceModel = new()
			{
				Name = name
			};

			this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
			this.MapperMock.Setup(p => p.Map<ReadTechnologyServiceModel>(It.IsAny<Technology>())).Returns(readTechnologyServiceModel);

			ReadTechnologyServiceModel result = await this.TechnologyService.GetTechnologyById(id);

			Assert.AreEqual(name, result.Name);
		}

		[Test]
		public void GetTechnologyById_ThrowsException_WhenTechnologyDoesNotExist()
		{
			string exceptionMessage = "The technology does not exist";
			Guid id = new Guid();
			this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Technology>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.TechnologyService.GetTechnologyById(id));

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

			this.TechnologyRepositoryMock.Setup(p => p.GetTechnologies()).Returns(new HashSet<Technology>());
			this.MapperMock.Setup(p => p.Map<HashSet<ReadTechnologyServiceModel>>(It.IsAny<HashSet<Technology>>())).Returns(technologies);

			HashSet<ReadTechnologyServiceModel> result = this.TechnologyService.GetTechnologies();

			Assert.GreaterOrEqual(2, result.Count, "GetTechnologies does not return all technologies");
		}

		[Test]
		public void GetLanguages_ReturnsEmptyHashSet_IfNoLanguagesExist()
		{
			this.TechnologyRepositoryMock.Setup(p => p.GetTechnologies()).Returns(new HashSet<Technology>());
			this.MapperMock.Setup(p => p.Map<HashSet<ReadTechnologyServiceModel>>(It.IsAny<HashSet<Technology>>())).Returns(new HashSet<ReadTechnologyServiceModel>());

			HashSet<ReadTechnologyServiceModel> result = this.TechnologyService.GetTechnologies();

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

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.TechnologyRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));
			this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<UpdateTechnologyServiceModel>())).Returns(technology);

			bool result = await this.TechnologyService.UpdateTechnology(updatetechnologyServiceModel);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void UpdateTechnology_ThrowsException_WhenTechnologyDoesNotExist()
		{
			string exceptionMessage = "Technology does not exist!";
			UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
			{
			};

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.TechnologyService.UpdateTechnology(updateTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}

		[Test]
		public void UpdateTechnology_ThrowsException_WhenTechnologyNameAlreadyExists()
		{
			string exceptionMessage = "Technology name already exists!";
			UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
			{
			};

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.TechnologyService.UpdateTechnology(updateTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteTechnology

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteTechnology_ShouldReturnIfDeletionIsSuccessfull_WhenTechnologyExists(bool shouldPass)
		{
			Guid id = new Guid();
			Technology technology = new Technology();

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
			this.TechnologyRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));

			bool result = await this.TechnologyService.DeleteTechnology(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteTechnology_ThrowsException_WhenTechnologyDoesNotExist()
		{
			string exceptionMessage = "Technology does not exist!";
			Guid id = new Guid();

			this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.TechnologyService.DeleteTechnology(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
