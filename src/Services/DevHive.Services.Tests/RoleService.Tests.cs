using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Models.Identity.Role;
using DevHive.Services.Services;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class RoleServiceTests
	{
		private Mock<IRoleRepository> RoleRepositoryMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private RoleService RoleService { get; set; }

		#region SetUps
		[SetUp]
		public void Setup()
		{
			this.RoleRepositoryMock = new Mock<IRoleRepository>();
			this.MapperMock = new Mock<IMapper>();
			this.RoleService = new RoleService(this.RoleRepositoryMock.Object, this.MapperMock.Object);
		}
		#endregion

		#region CreateRole
		[Test]
		public async Task CreateRole_ReturnsNonEmptyGuid_WhenEntityIsAddedSuccessfully()
		{
			string roleName = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			CreateRoleServiceModel createRoleServiceModel = new CreateRoleServiceModel
			{
				Name = roleName
			};
			Role role = new()
			{
				Name = roleName,
				Id = id
			};

			this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.RoleRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Role>())).Returns(Task.FromResult(true));
			this.RoleRepositoryMock.Setup(p => p.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(role));
			this.MapperMock.Setup(p => p.Map<Role>(It.IsAny<CreateRoleServiceModel>())).Returns(role);

			Guid result = await this.RoleService.CreateRole(createRoleServiceModel);

			Assert.AreEqual(id, result);
		}

		[Test]
		public async Task CreateRoley_ReturnsEmptyGuid_WhenEntityIsNotAddedSuccessfully()
		{
			string roleName = "Gosho Trapov";

			CreateRoleServiceModel createRoleServiceModel = new CreateRoleServiceModel
			{
				Name = roleName
			};
			Role role = new Role
			{
				Name = roleName
			};

			this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.RoleRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Role>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<Role>(It.IsAny<CreateRoleServiceModel>())).Returns(role);

			Guid result = await this.RoleService.CreateRole(createRoleServiceModel);

			Assert.IsTrue(result == Guid.Empty);
		}

		[Test]
		public void CreateTechnology_ThrowsArgumentException_WhenEntityAlreadyExists()
		{
			string exceptionMessage = "Role already exists!";
			string roleName = "Gosho Trapov";

			CreateRoleServiceModel createRoleServiceModel = new CreateRoleServiceModel
			{
				Name = roleName
			};
			Role role = new Role
			{
				Name = roleName
			};

			this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RoleService.CreateRole(createRoleServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region GetRoleById
		[Test]
		public async Task GetRoleById_ReturnsTheRole_WhenItExists()
		{
			Guid id = new Guid();
			string name = "Gosho Trapov";
			Role role = new Role
			{
				Name = name
			};
			RoleServiceModel roleServiceModel = new RoleServiceModel
			{
				Name = name
			};

			this.RoleRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(role));
			this.MapperMock.Setup(p => p.Map<RoleServiceModel>(It.IsAny<Role>())).Returns(roleServiceModel);

			RoleServiceModel result = await this.RoleService.GetRoleById(id);

			Assert.AreEqual(name, result.Name);
		}

		[Test]
		public void GetRoleById_ThrowsException_WhenRoleDoesNotExist()
		{
			string exceptionMessage = "Role does not exist!";
			Guid id = new Guid();
			this.RoleRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Role>(null));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RoleService.GetRoleById(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region UpdateRole
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task UpdateRole_ReturnsIfUpdateIsSuccessfull_WhenRoleExistsy(bool shouldPass)
		{
			string name = "Gosho Trapov";
			Guid id = Guid.NewGuid();
			Role role = new Role
			{
				Name = name,
				Id = id
			};
			UpdateRoleServiceModel updateRoleServiceModel = new UpdateRoleServiceModel
			{
				Name = name,
			};

			this.RoleRepositoryMock.Setup(p => p.DoesRoleExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(false));
			this.RoleRepositoryMock.Setup(p => p.EditAsync(It.IsAny<Guid>(), It.IsAny<Role>())).Returns(Task.FromResult(shouldPass));
			this.MapperMock.Setup(p => p.Map<Role>(It.IsAny<UpdateRoleServiceModel>())).Returns(role);

			bool result = await this.RoleService.UpdateRole(updateRoleServiceModel);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void UpdateRole_ThrowsException_WhenRoleDoesNotExist()
		{
			string exceptionMessage = "Role does not exist!";
			UpdateRoleServiceModel updateRoleServiceModel = new UpdateRoleServiceModel
			{
			};

			this.RoleRepositoryMock.Setup(p => p.DoesRoleExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RoleService.UpdateRole(updateRoleServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}

		[Test]
		public void UpdateRole_ThrowsException_WhenRoleNameAlreadyExists()
		{
			string exceptionMessage = "Role name already exists!";
			UpdateRoleServiceModel updateRoleServiceModel = new UpdateRoleServiceModel
			{
			};

			this.RoleRepositoryMock.Setup(p => p.DoesRoleExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RoleRepositoryMock.Setup(p => p.DoesNameExist(It.IsAny<string>())).Returns(Task.FromResult(true));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RoleService.UpdateRole(updateRoleServiceModel));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion

		#region DeleteRole
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public async Task DeleteRole_ShouldReturnIfDeletionIsSuccessfull_WhenRoleExists(bool shouldPass)
		{
			Guid id = new Guid();
			Role role = new Role();

			this.RoleRepositoryMock.Setup(p => p.DoesRoleExist(It.IsAny<Guid>())).Returns(Task.FromResult(true));
			this.RoleRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(role));
			this.RoleRepositoryMock.Setup(p => p.DeleteAsync(It.IsAny<Role>())).Returns(Task.FromResult(shouldPass));

			bool result = await this.RoleService.DeleteRole(id);

			Assert.AreEqual(shouldPass, result);
		}

		[Test]
		public void DeleteRole_ThrowsException_WhenRoleDoesNotExist()
		{
			string exceptionMessage = "Role does not exist!";
			Guid id = new Guid();

			this.RoleRepositoryMock.Setup(p => p.DoesRoleExist(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			Exception ex = Assert.ThrowsAsync<ArgumentException>(() => this.RoleService.DeleteRole(id));

			Assert.AreEqual(exceptionMessage, ex.Message, "Incorecct exception message");
		}
		#endregion
	}
}
