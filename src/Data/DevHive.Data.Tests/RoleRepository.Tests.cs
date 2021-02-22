using System;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class RoleRepositoryTests
	{
		private const string ROLE_NAME = "Role test name";
		private DevHiveContext _context;
		private RoleRepository _roleRepository;

		#region Setups
		[SetUp]
		public void Setup()
		{
			DbContextOptionsBuilder<DevHiveContext> optionsBuilder = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase(databaseName: "DevHive_Test_Database");

			this._context = new DevHiveContext(optionsBuilder.Options);

			Mock<RoleManager<Role>> roleManagerMock = new();
			this._roleRepository = new(this._context, roleManagerMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_ = this._context.Database.EnsureDeleted();
		}
		#endregion

		#region GetByNameAsync
		[Test]
		public async Task GetByNameAsync_ReturnsTheRole_WhenItExists()
		{
			Role role = await this.AddEntity();

			Role resultRole = await this._roleRepository.GetByNameAsync(role.Name);

			Assert.AreEqual(role.Id, resultRole.Id, "GetByNameAsync does not return the correct role");
		}

		[Test]
		public async Task GetByNameAsync_ReturnsNull_WhenTheRoleDoesNotExist()
		{
			Role resultRole = await this._roleRepository.GetByNameAsync(ROLE_NAME);

			Assert.IsNull(resultRole, "GetByNameAsync does not return when the role does not exist");
		}
		#endregion

		#region DoesNameExist
		[Test]
		public async Task DoesNameExist_ReturnsTrue_WhenTheNameExists()
		{
			Role role = await this.AddEntity();

			bool result = await this._roleRepository.DoesNameExist(role.Name);

			Assert.IsTrue(result, "DoesNameExist returns false when the role name exist");
		}

		[Test]
		public async Task DoesNameExist_ReturnsFalse_WhenTheNameDoesNotExist()
		{
			bool result = await this._roleRepository.DoesNameExist(ROLE_NAME);

			Assert.IsFalse(result, "DoesNameExist returns false when the role name exist");
		}
		#endregion

		#region DoesRoleExist
		[Test]
		public async Task DoesRoleExist_ReturnsTrue_IfIdExists()
		{
			_ = await this.AddEntity();
			Role role = this._context.Roles.Where(x => x.Name == ROLE_NAME).AsEnumerable().FirstOrDefault();
			Guid id = role.Id;

			bool result = await this._roleRepository.DoesRoleExist(id);

			Assert.IsTrue(result, "DoesRoleExistAsync returns flase when role exists");
		}

		[Test]
		public async Task DoesRoleExist_ReturnsFalse_IfIdDoesNotExists()
		{
			Guid id = Guid.NewGuid();

			bool result = await this._roleRepository.DoesRoleExist(id);

			Assert.IsFalse(result, "DoesRoleExist returns true when role does not exist");
		}
		#endregion

		#region HelperMethods
		private async Task<Role> AddEntity(string name = ROLE_NAME)
		{
			Role role = new()
			{
				Id = Guid.NewGuid(),
				Name = name
			};

			_ = this._context.Roles.Add(role);
			_ = await this._context.SaveChangesAsync();

			return role;
		}
		#endregion
	}
}
