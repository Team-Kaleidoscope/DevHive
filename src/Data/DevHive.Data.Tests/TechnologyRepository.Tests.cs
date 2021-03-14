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
		private DevHiveContext _context;
		private TechnologyRepository _technologyRepository;

		#region Setups
		[SetUp]
		public void Setup()
		{
			DbContextOptionsBuilder<DevHiveContext> optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this._context = new DevHiveContext(optionsBuilder.Options);

			this._technologyRepository = new TechnologyRepository(this._context);
		}

		[TearDown]
		public void TearDown()
		{
			_ = this._context.Database.EnsureDeleted();
		}
		#endregion

		#region GetByNameAsync
		[Test]
		public async Task GetByNameAsync_ReturnsTheCorrectTechnology_IfItExists()
		{
			await this.AddEntity();

			Technology technology = this._context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).AsEnumerable().FirstOrDefault();

			Technology resultTechnology = await this._technologyRepository.GetByNameAsync(TECHNOLOGY_NAME);

			Assert.AreEqual(technology.Id, resultTechnology.Id);
		}

		[Test]
		public async Task GetByNameAsync_ReturnsNull_IfTechnologyDoesNotExists()
		{
			Technology resultTechnology = await this._technologyRepository.GetByNameAsync(TECHNOLOGY_NAME);

			Assert.IsNull(resultTechnology);
		}
		#endregion

		#region DoesTechnologyExistAsync
		[Test]
		public async Task DoesTechnologyExist_ReturnsTrue_IfIdExists()
		{
			await this.AddEntity();
			Technology technology = this._context.Technologies.Where(x => x.Name == TECHNOLOGY_NAME).AsEnumerable().FirstOrDefault();
			Guid id = technology.Id;

			bool result = await this._technologyRepository.DoesTechnologyExistAsync(id);

			Assert.IsTrue(result, "DoesTechnologyExistAsync returns flase hwen technology exists");
		}

		[Test]
		public async Task DoesTechnologyExist_ReturnsFalse_IfIdDoesNotExists()
		{
			Guid id = Guid.NewGuid();

			bool result = await this._technologyRepository.DoesTechnologyExistAsync(id);

			Assert.IsFalse(result, "DoesTechnologyExistAsync returns true when technology does not exist");
		}
		#endregion

		#region DoesTechnologyNameExistAsync
		[Test]
		public async Task DoesTechnologyNameExist_ReturnsTrue_IfTechnologyExists()
		{
			await this.AddEntity();

			bool result = await this._technologyRepository.DoesTechnologyNameExistAsync(TECHNOLOGY_NAME);

			Assert.IsTrue(result, "DoesTechnologyNameExists returns true when technology name does not exist");
		}

		[Test]
		public async Task DoesTechnologyNameExist_ReturnsFalse_IfTechnologyDoesNotExists()
		{
			bool result = await this._technologyRepository.DoesTechnologyNameExistAsync(TECHNOLOGY_NAME);

			Assert.False(result, "DoesTechnologyNameExistAsync returns true when technology name does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task AddEntity(string name = TECHNOLOGY_NAME)
		{
			Technology technology = new()
			{
				Name = name
			};

			_ = this._context.Technologies.Add(technology);
			_ = await this._context.SaveChangesAsync();
		}
		#endregion
	}
}
