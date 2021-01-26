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

		#region Setups
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
		#endregion

		#region GetByNameAsync
		[Test]
		public async Task GetByNameAsync_ReturnsTheCorrectTechnology_IfItExists()
		{
			await AddEntity();

			Technology technology = this.Context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).ToList().FirstOrDefault();

			Technology resultTechnology = await this.TechnologyRepository.GetByNameAsync(TECHNOLOGY_NAME);

			Assert.AreEqual(technology.Id, resultTechnology.Id);
		}

		[Test]
		public async Task GetByNameAsync_ReturnsNull_IfTechnologyDoesNotExists()
		{
			Technology resultTechnology = await this.TechnologyRepository.GetByNameAsync(TECHNOLOGY_NAME);

			Assert.IsNull(resultTechnology);
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

		#region HelperMethods
		private async Task AddEntity(string name = TECHNOLOGY_NAME)
		{
			Technology technology = new Technology
			{
				Name = name
			};

			this.Context.Technologies.Add(technology);
			await this.Context.SaveChangesAsync();
		}
		#endregion

		//Task.Run(async () =>
		//{

		//}).GetAwaiter().GetResult();
	}
}
