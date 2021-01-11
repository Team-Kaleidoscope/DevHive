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
        public void Create_ReturnsTrue_WhenEntityIsAddedSuccessfully()
        {            
            Task.Run(async () =>
            {
                TechnologyServiceModel technologyServiceModel = new TechnologyServiceModel
                {
                    Name = "Some name"
                };
                Technology technology = new Technology
                {
                    Name = "Some name"
                };

                this.TechnologyRepositoryMock.Setup(p => p.DoesTechnologyNameExist(It.IsAny<string>())).Returns(Task.FromResult(false));
                this.TechnologyRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Technology>())).Returns(Task.FromResult(true));
                this.MapperMock.Setup(p => p.Map<Technology>(It.IsAny<TechnologyServiceModel>())).Returns(technology);

                bool result = await this.TechnologyService.Create(technologyServiceModel);

                Assert.IsTrue(result, "Create returns false when entity is created successfully");
            }).GetAwaiter().GetResult();
        }


        #endregion

        [Test]
        public void Test()
        {
            Assert.IsTrue(true);
        }
    }
}
