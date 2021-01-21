using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models.Technology;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class TechnologyServicesTests
	{
		private Mock<ITechnologyRepository> TechnologyRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private TechnologyService TechnologyService { get; set; }

		[SetUp]
		public void Setup()
		{
			this.TechnologyRepositoryMock = new Mock<ITechnologyRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.TechnologyService = new TechnologyService(this.TechnologyRepositoryMock.Object, this.MapperMock.Object);
		}

		#region Create

		[Test]
		public void Create_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			Task.Run(async () =>
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

				Guid result = await this.TechnologyService.Create(createTechnologyServiceModel);

				Assert.AreEqual(id, result);
			}).GetAwaiter().GetResult();
		}

		[Test]
		public void Create_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			Task.Run(async () =>
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

				Guid result = await this.TechnologyService.Create(createTechnologyServiceModel);

				Assert.IsTrue(result == Guid.Empty);
			}).GetAwaiter().GetResult();
		}


		[Test]
		public void Create_ThrowsArgumentException_WhenEntityAlreadyExists()
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

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.TechnologyService.Create(createTechnologyServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region Read

		[Test]
		public void GetTechnologyById_ReturnsTheTechnology_WhenItExists()
		{
			Task.Run(async () =>
			{
				Guid id = new Guid();
				string name = "Gosho Trapov";
				Technology technology = new()
				{
					Name = name
				};
				CreateTechnologyServiceModel createTechnologyServiceModel = new()
				{
					Name = name
				};

				this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
				this.MapperMock.Setup(p => p.Map<CreateTechnologyServiceModel>(It.IsAny<Technology>())).Returns(createTechnologyServiceModel);

				CreateTechnologyServiceModel result = await this.TechnologyService.GetTechnologyById(id);

				Assert.AreEqual(name, result.Name);
			}).GetAwaiter().GetResult();
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

		#region Update

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void UpdateTechnology_ReturnsIfUpdateIsSuccessfull_WhenTechnologyExistsy(bool shouldPass)
		{
			Task.Run(async () =>
			{
				string name = "Gosho Trapov";
				Technology technology = new Technology
				{
					Name = name
				};
				UpdateTechnologyServiceModel updatetechnologyServiceModel = new UpdateTechnologyServiceModel
				{
					Name = name,
				};

				this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
				this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExistAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
				this.TechnologyRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));
				this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<UpdateTechnologyServiceModel>())).Returns(technology);

				bool result = await this.TechnologyService.UpdateTechnology(updatetechnologyServiceModel);

				Assert.AreEqual(shouldPass, result);
			}).GetAwaiter().GetResult();
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

		#region Delete

		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void DeleteTechnology_ShouldReturnIfDeletionIsSuccessfull_WhenTechnologyExists(bool shouldPass)
		{
			Task.Run(async () =>
			{
				Guid id = new Guid();
				Technology technology = new Technology();

				this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
				this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
				this.TechnologyRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));

				bool result = await this.TechnologyService.DeleteTechnology(id);

				Assert.AreEqual(shouldPass, result);
			}).GetAwaiter().GetResult();
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
		//Task.Run(async () =>
		//{
		//
		//}).GetAwaiter().GetResult();
	}
}
