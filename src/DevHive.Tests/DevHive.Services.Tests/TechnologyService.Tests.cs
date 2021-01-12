using AutoMapper;
using DevHive.Data;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Data.Repositories.Contracts;
using DevHive.Services.Models.Technology;
using DevHive.Services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DevHive.Services.Tests
{
    [TestFixture]
    public class TechnologyServiceTests
    {
        protected Mock<ITechnologyRepository> TechnologyRepositoryMock { get; set; }
        protected Mock<IMapper> MapperMock { get; set; }
        protected TechnologyService TechnologyService { get; set; }

        [SetUp]
        public void Setup()
        {
            this.TechnologyRepositoryMock = new Mock<ITechnologyRepository>();
            this.MapperMock = new Mock<IMapper>();
            this.TechnologyService = new TechnologyService(this.TechnologyRepositoryMock.Object, this.MapperMock.Object);
        }

        #region Create
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Create_ReturnsTrue_WhenEntityIsAddedSuccessfully(bool shouldFail)
        {            
            Task.Run(async () =>
            {
                string technologyName = "Gosho Trapov";

                TechnologyServiceModel technologyServiceModel = new TechnologyServiceModel
                {
                    Name = technologyName
                };
                Technology technology = new Technology
                {
                    Name = technologyName
                };

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExist(It.IsAny<string>())).Returns(Task.FromResult(false));
                this.TechnologyRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Technology>())).Returns(Task.FromResult(shouldFail));
                this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<TechnologyServiceModel>())).Returns(technology);

                bool result = await this.TechnologyService.Create(technologyServiceModel);

                Assert.AreEqual(shouldFail, result);
            }).GetAwaiter().GetResult();
        }       

        [Test]
        public void Create_ThrowsArgumentException_WhenEntityAlreadyExists()
        {
            Task.Run(async () =>
            {
                string expectedExceptionMessage = "Technology already exists!";
                string technologyName = "Gosho Trapov";

                TechnologyServiceModel technologyServiceModel = new TechnologyServiceModel
                {
                    Name = technologyName
                };
                Technology technology = new Technology
                {
                    Name = technologyName
                };

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExist(It.IsAny<string>())).Returns(Task.FromResult(true));

                try
                {
                    await this.TechnologyService.Create(technologyServiceModel);
                    Assert.Fail("Create does not throw exception when technology already exists");
                }
                catch(ArgumentException ex)
                {
                    Assert.AreEqual(expectedExceptionMessage, ex.Message);
                }
            }).GetAwaiter().GetResult();
        }
        #endregion

        #region GetById
        [Test]
        public void GetTechnologyById_ReturnsTheTechnology_WhenItExists()
        {
            Task.Run(async () =>
            {
                Guid id = new Guid();
                string name = "Gosho Trapov";
                Technology technology = new Technology
                {
                    Name = name
                };
                TechnologyServiceModel technologyServiceModel = new TechnologyServiceModel
                {
                    Name = name
                };

                this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(technology));
                this.MapperMock.Setup(p => p.Map<TechnologyServiceModel>(It.IsAny<Technology>())).Returns(technologyServiceModel);

                TechnologyServiceModel result = await this.TechnologyService.GetTechnologyById(id);

                Assert.AreEqual(name, result.Name);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void GetTechnologyById_ThrowsException_WhenTechnologyDoesNotExist()
        {
            Task.Run(async () =>
            {
                string exceptionMessage = "The technology does not exist";
                Guid id = new Guid();
                this.TechnologyRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Technology>(null));

                try
                {
                    await this.TechnologyService.GetTechnologyById(id);
                    Assert.Fail("GetTechnologyById does not throw exception when technology does not exist");
                }
                catch(ArgumentException ex)
                {
                    Assert.AreEqual(exceptionMessage, ex.Message, "Exception messege is nto correct");
                }
            }).GetAwaiter().GetResult();
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
                Guid id = new Guid();
                string name = "Gosho Trapov";
                Technology technology = new Technology
                {
                    Name = name
                };
                UpdateTechnologyServiceModel updatetechnologyServiceModel = new UpdateTechnologyServiceModel
                {
                    Name = name,
                    Id = id
                };

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExist(It.IsAny<string>())).Returns(Task.FromResult(false));
                this.TechnologyRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Technology>())).Returns(Task.FromResult(shouldPass));
                this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<UpdateTechnologyServiceModel>())).Returns(technology);

                bool result = await this.TechnologyService.UpdateTechnology(updatetechnologyServiceModel);

                Assert.AreEqual(shouldPass, result);
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void UpdateTechnology_ThrowsException_WhenTechnologyDoesNotExist()
        {
            Task.Run(async () =>
            {
                string exceptionMessage = "Technology does not exist!";
                Guid id = new Guid();
                UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
                {
                    Id = id
                };

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

                try
                {
                    await this.TechnologyService.UpdateTechnology(updateTechnologyServiceModel);
                    Assert.Fail("UpdateTechnology does not throw exception when technology does not exist"); 
                }
                catch(ArgumentException ex)
                {
                    Assert.AreEqual(exceptionMessage, ex.Message, "Exception Message is not correct");
                }
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void UpdateTechnology_ThrowsException_WhenTechnologyNameAlreadyExists()
        {
            Task.Run(async () =>
            {
                string exceptionMessage = "Technology name already exists!";
                Guid id = new Guid();
                UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
                {
                    Id = id
                };

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));
                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExist(It.IsAny<string>())).Returns(Task.FromResult(true));

                try
                {
                    await this.TechnologyService.UpdateTechnology(updateTechnologyServiceModel);
                    Assert.Fail("UpdateTechnology does not throw exception when technology name already exist");
                }
                catch(ArgumentException ex)
                {
                    Assert.AreEqual(exceptionMessage, ex.Message, "Exception Message is not correct");
                }
            }).GetAwaiter().GetResult();
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
            Task.Run(async () =>
            {
                string exceptionMessage = "Technology does not exist!";
                Guid id = new Guid();

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyExistAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

                try
                {
                    await this.TechnologyService.DeleteTechnology(id);
                    Assert.Fail("DeleteTechnology does not throw exception when technology does not exist");
                }
                catch(ArgumentException ex)
                {
                    Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
                }
            }).GetAwaiter().GetResult();
        }
        #endregion
        //Task.Run(async () =>
        //{
        //
        //}).GetAwaiter().GetResult();
    }
}
