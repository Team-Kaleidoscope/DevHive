using System;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class RoleRepositoryTests
	{
		private const string ROLE_NAME = "Role test name";

		protected DevHiveContext Context { get; set; }

		protected RoleRepository RoleRepository { get; set; }

		#region Setups
		[SetUp]
		public void Setup()
		{
			var optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this.Context = new DevHiveContext(optionsBuilder.Options);

			RoleRepository = new RoleRepository(Context);
		}

		[TearDown]
		public void TearDown()
		{
			this.Context.Database.EnsureDeleted();
		}
		#endregion

		#region GetByNameAsync
		[Test]
		public async Task GetByNameAsync_ReturnsTheRole_WhenItExists()
		{
			Role role = await this.AddEntity();

			Role resultRole = await this.RoleRepository.GetByNameAsync(role.Name);

			Assert.AreEqual(role.Id, resultRole.Id, "GetByNameAsync does not return the correct role");
		}

		[Test]
		public async Task GetByNameAsync_ReturnsNull_WhenTheRoleDoesNotExist()
		{
			Role resultRole = await this.RoleRepository.GetByNameAsync(ROLE_NAME);

			Assert.IsNull(resultRole, "GetByNameAsync does not return when the role does not exist");
		}
		#endregion

		#region DoesNameExist
		[Test]
		public async Task DoesNameExist_ReturnsTrue_WhenTheNameExists()
		{
			Role role = await this.AddEntity();

			bool result = await this.RoleRepository.DoesNameExist(role.Name);

			Assert.IsTrue(result, "DoesNameExist returns false when the role name exist");
		}

		[Test]
		public async Task DoesNameExist_ReturnsFalse_WhenTheNameDoesNotExist()
		{
			bool result = await this.RoleRepository.DoesNameExist(ROLE_NAME);

			Assert.IsFalse(result, "DoesNameExist returns false when the role name exist");
		}
		#endregion

		#region DoesRoleExist
		[Test]
		public async Task DoesRoleExist_ReturnsTrue_IfIdExists()
		{
			await AddEntity();
			Role role = this.Context.Roles.Where(x => x.Name == ROLE_NAME).ToList().FirstOrDefault();
			Guid id = role.Id;

			bool result = await this.RoleRepository.DoesRoleExist(id);

			Assert.IsTrue(result, "DoesRoleExistAsync returns flase when role exists");
		}

		[Test]
		public async Task DoesRoleExist_ReturnsFalse_IfIdDoesNotExists()
		{
			Guid id = Guid.NewGuid();

			bool result = await this.RoleRepository.DoesRoleExist(id);

			Assert.IsFalse(result, "DoesRoleExist returns true when role does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task<Role> AddEntity(string name = ROLE_NAME)
		{
			Role role = new Role
			{
				Id = Guid.NewGuid(),
				Name = name
			};

			this.Context.Roles.Add(role);
			await this.Context.SaveChangesAsync();

			return role;
		}
		#endregion
	}
}
