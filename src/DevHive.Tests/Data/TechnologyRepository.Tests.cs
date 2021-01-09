using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using DevHive.Data;
using DevHive.Data.Repositories;
using DevHive.Data.Models;
using System.Threading.Tasks;
using System.Linq;

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

			AddEntity();
		}

		[Test]
		public void AddAsync_AddsTheGivenTechnologyToTheDatabase()
		{
			int numberOfTechnologies = Context.Technologies.Count();

			Assert.True(numberOfTechnologies > 0, "Technologies repo does not store Technologies correctly");
		}

		private void AddEntity()
		{
			Task.Run(async () =>
			{
				Technology technology = new Technology
				{
					Name = TECHNOLOGY_NAME
				};

				await this.TechnologyRepository.AddAsync(technology);		
			}).GetAwaiter().GetResult();
		}		
	}
}