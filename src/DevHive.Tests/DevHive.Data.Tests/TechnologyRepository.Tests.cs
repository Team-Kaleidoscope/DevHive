using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevHive.Data.Tests
{
    [TestFixture]
    public class TechnologyRepositoryTests
    {
        private const string TECHNOLOGY_NAME = "Technology test name";

        protected DevHiveContext Context { get; set; }

        protected TechnologyRepository TechnologyRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
                .UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

            this.Context = new DevHiveContext(optionsBuilder.Options);

            TechnologyRepository = new TechnologyRepository(Context);
        }

        [TearDown]
        public void TearDown()
        {
            this.Context.Database.EnsureDeleted();
        }

        #region AddAsync
        [Test]
        public void AddAsync_AddsTheGivenTechnologyToTheDatabase()
        {
            AddEntity();

            int numberOfTechnologies = Context.Technologies.Count();

            Assert.True(numberOfTechnologies > 0, "Technologies repo does not store Technologies correctly");
        }
        #endregion

        #region GetByIdAsync
        [Test]
        public void GetByIdAsync_ReturnsTheCorrectTechnology_IfIdExists()
        {
            Task.Run(async () =>
            {
                AddEntity();
                Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();
                Guid id = technology.Id;

                Technology technologyReturned = await this.TechnologyRepository.GetByIdAsync(id);

                Assert.AreEqual(TECHNOLOGY_NAME, technologyReturned.Name, "GetByIdAsync does not return the correct Technology when id is valid");
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void GetByIdAsync_ReturnsNull_IfIdDoesNotExists()
        {
            Task.Run(async () =>
            {
                Guid id = new Guid();

                Technology technologyReturned = await this.TechnologyRepository.GetByIdAsync(id);

                Assert.IsNull(technologyReturned, "GetByIdAsync returns Technology when it should be null");
            }).GetAwaiter().GetResult();
        }
        #endregion

        #region DoesTechnologyExist
        [Test]
        public void DoesTechnologyExist_ReturnsTrue_IfIdExists()
        {
            Task.Run(async () =>
            {
                AddEntity();
                Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();
                Guid id = technology.Id;

                bool result = await this.TechnologyRepository.DoesTechnologyExist(id);

                Assert.IsTrue(result, "DoesTechnologyExist returns flase hwen technology exists");
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void DoesTechnologyExist_ReturnsFalse_IfIdDoesNotExists()
        {
            Task.Run(async () =>
            {
                Guid id = new Guid();

                bool result = await this.TechnologyRepository.DoesTechnologyExist(id);

                Assert.IsFalse(result, "DoesTechnologyExist returns true when technology does not exist");
            }).GetAwaiter().GetResult();
        }
        #endregion

        #region DoesTechnologyNameExist
        [Test]
        public void DoesTechnologyNameExist_ReturnsTrue_IfTechnologyExists()
        {
            Task.Run(async () =>
            {
                AddEntity();

                bool result = await this.TechnologyRepository.DoesTechnologyNameExist(TECHNOLOGY_NAME);

                Assert.IsTrue(result, "DoesTechnologyNameExists returns true when technology name does not exist");
            }).GetAwaiter().GetResult();
        }     

        [Test]
        public void DoesTechnologyNameExist_ReturnsFalse_IfTechnologyDoesNotExists()
        {
            Task.Run(async () =>
            {
                bool result = await this.TechnologyRepository.DoesTechnologyNameExist(TECHNOLOGY_NAME);

                Assert.False(result, "DoesTechnologyNameExist returns true when technology name does not exist");
            }).GetAwaiter().GetResult();
        }
        #endregion

        #region EditAsync
        [Test]
        public void EditAsync_UpdatesEntity()
        {
            Task.Run(async () =>
            {
                string newName = "New name";
                Guid id = new Guid();
                Technology technology = new Technology
                {
                    Name = TECHNOLOGY_NAME,
                    Id = id
                };
                Technology newTechnology = new Technology
                {
                    Name = newName,
                    Id = id
                };
                await this.TechnologyRepository.AddAsync(technology);

                bool result = await this.TechnologyRepository.EditAsync(newTechnology);

                Assert.IsTrue(result);
            }).GetAwaiter().GetResult();
        }
        #endregion

        #region DeleteAsync
        [Test]
        public void DeleteAsync_ReturnsTrue_IfDeletionIsSuccesfull()
        {           
            Task.Run(async () =>
            {
                AddEntity();
                Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();

                bool result = await this.TechnologyRepository.DeleteAsync(technology);

                Assert.IsTrue(result, "DeleteAsync returns false when deletion is successfull");

            }).GetAwaiter().GetResult();
        }       
        #endregion

        #region HelperMethods
        private void AddEntity(string name = TECHNOLOGY_NAME)
        {
            Task.Run(async () =>
            {
                Technology technology = new Technology
                {
                    Name = name
                };

                await this.TechnologyRepository.AddAsync(technology);
            }).GetAwaiter().GetResult();
        }
        #endregion

        //Task.Run(async () =>
        //{
        //
        //}).GetAwaiter().GetResult();
    }
}
