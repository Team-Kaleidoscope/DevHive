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
		public async Task AddAsync_AddsTheGivenTechnologyToTheDatabase()
		{
			await AddEntity();

			int numberOfTechnologies = Context.Technologies.Count();

			Assert.True(numberOfTechnologies > 0, "Technologies repo does not store Technologies correctly");
		}
		#endregion

		#region GetByIdAsync
		[Test]
		public async Task GetByIdAsync_ReturnsTheCorrectTechnology_IfIdExists()
		{
			await AddEntity();
			Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();
			Guid id = technology.Id;

			Technology technologyReturned = await this.TechnologyRepository.GetByIdAsync(id);

			Assert.AreEqual(TECHNOLOGY_NAME, technologyReturned.Name, "GetByIdAsync does not return the correct Technology when id is valid");
		}

		[Test]
		public async Task GetByIdAsync_ReturnsNull_IfIdDoesNotExists()
		{
			Guid id = Guid.NewGuid();

			Technology technologyReturned = await this.TechnologyRepository.GetByIdAsync(id);

			Assert.IsNull(technologyReturned, "GetByIdAsync returns Technology when it should be null");
		}
		#endregion

		#region DoesTechnologyExistAsync
		[Test]
		public async Task DoesTechnologyExist_ReturnsTrue_IfIdExists()
		{
			await AddEntity();
			Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();
			Guid id = technology.Id;

			bool result = await this.TechnologyRepository.DoesTechnologyExistAsync(id);

			Assert.IsTrue(result, "DoesTechnologyExistAsync returns flase hwen technology exists");
		}

		[Test]
		public async Task DoesTechnologyExist_ReturnsFalse_IfIdDoesNotExists()
		{
			Guid id = Guid.NewGuid();

			bool result = await this.TechnologyRepository.DoesTechnologyExistAsync(id);

			Assert.IsFalse(result, "DoesTechnologyExistAsync returns true when technology does not exist");
		}
		#endregion

		#region DoesTechnologyNameExistAsync
		[Test]
		public async Task DoesTechnologyNameExist_ReturnsTrue_IfTechnologyExists()
		{
			await AddEntity();

			bool result = await this.TechnologyRepository.DoesTechnologyNameExistAsync(TECHNOLOGY_NAME);

			Assert.IsTrue(result, "DoesTechnologyNameExists returns true when technology name does not exist");
		}

		[Test]
		public async Task DoesTechnologyNameExist_ReturnsFalse_IfTechnologyDoesNotExists()
		{
			bool result = await this.TechnologyRepository.DoesTechnologyNameExistAsync(TECHNOLOGY_NAME);

			Assert.False(result, "DoesTechnologyNameExistAsync returns true when technology name does not exist");
		}
		#endregion

		#region EditAsync
		//TO DO fix: check UserRepo
		[Test]
		public async Task EditAsync_UpdatesEntity()
		{
			string newName = "New name";
			Technology technology = new Technology
			{
				Name = TECHNOLOGY_NAME
			}; Technology newTechnology = new Technology
			{
				Name = newName
			};

			await this.TechnologyRepository.AddAsync(technology);

			bool result = await this.TechnologyRepository.EditAsync(newTechnology);

			Assert.IsTrue(result);
		}
		#endregion

		#region DeleteAsync
		[Test]
		public async Task DeleteAsync_ReturnsTrue_IfDeletionIsSuccesfull()
		{
			await AddEntity();
			Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();

			bool result = await this.TechnologyRepository.DeleteAsync(technology);

			Assert.IsTrue(result, "DeleteAsync returns false when deletion is successfull");
		}
		#endregion

		#region HelperMethods
		private async Task AddEntity(string name = TECHNOLOGY_NAME)
		{
			Technology technology = new Technology
			{
				Name = name
			};

			await this.TechnologyRepository.AddAsync(technology);
		}
		#endregion

		//Task.Run(async () =>
		//{

		//}).GetAwaiter().GetResult();
	}
}
