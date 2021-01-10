using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using DevHive.Data;
using DevHive.Data.Repositories;
using DevHive.Data.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Cryptography.X509Certificates;

namespace DevHive.Tests.Data
{
	[TestFixture]
	public class TechnologyRepositoryTests
	{
		private const string TECHNOLOGY_NAME = "Technology test name";

		protected DevHiveContext Context { get;set; }
		
		protected TechnologyRepository TechnologyRepository { get;set; }

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

		[Test]
		public void AddAsync_AddsTheGivenTechnologyToTheDatabase()
		{
			AddEntity();
			
			int numberOfTechnologies = Context.Technologies.Count();

			Assert.True(numberOfTechnologies > 0, "Technologies repo does not store Technologies correctly");
		}

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

				Assert.False(result, "DoesTechnologyNameExist returns true when tehcnology name does not exist");
			}).GetAwaiter().GetResult();
		}

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

		//Task.Run(async () =>
		//{
		//
		//}).GetAwaiter().GetResult();
	}
}