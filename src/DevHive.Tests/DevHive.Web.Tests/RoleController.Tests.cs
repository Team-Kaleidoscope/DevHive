using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Identity.Role;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Identity.Role;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
    [TestFixture]
	public class RoleControllerTests
	{
		const string NAME = "Gosho Trapov";
		private Mock<IRoleService> RoleServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private RoleController RoleController { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.RoleServiceMock = new Mock<IRoleService>();
			this.MapperMock = new Mock<IMapper>();
			this.RoleController = new RoleController(this.RoleServiceMock.Object, this.MapperMock.Object);
		}

		#region Create
		[Test]
		public void CreateRole_ReturnsOkObjectResult_WhenRoleIsSuccessfullyCreated()
		{
			CreateRoleWebModel createRoleWebModel = new CreateRoleWebModel
			{
				Name = NAME
			};
			CreateRoleServiceModel createRoleServiceModel = new CreateRoleServiceModel
			{
				Name = NAME
			};
			Guid id = Guid.NewGuid();

			this.MapperMock.Setup(p => p.Map<CreateRoleServiceModel>(It.IsAny<CreateRoleWebModel>())).Returns(createRoleServiceModel);
			this.RoleServiceMock.Setup(p => p.CreateRole(It.IsAny<CreateRoleServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.RoleController.Create(createRoleWebModel).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			var splitted = (result as OkObjectResult).Value
				.ToString()
				.Split('{', '}', '=', ' ')
				.Where(x => !string.IsNullOrEmpty(x))
				.ToArray();

			Guid resultId = Guid.Parse(splitted[1]);

			Assert.AreEqual(id, resultId);
		}

		[Test]
		public void CreateRole_ReturnsBadRequestObjectResult_WhenRoleIsNotCreatedSuccessfully()
		{
			CreateRoleWebModel createTechnologyWebModel = new CreateRoleWebModel
			{
				Name = NAME
			};
			CreateRoleServiceModel createTechnologyServiceModel = new CreateRoleServiceModel
			{
				Name = NAME
			};
			Guid id = Guid.Empty;
			string errorMessage = $"Could not create role {NAME}";

			this.MapperMock.Setup(p => p.Map<CreateRoleServiceModel>(It.IsAny<CreateRoleWebModel>())).Returns(createTechnologyServiceModel);
			this.RoleServiceMock.Setup(p => p.CreateRole(It.IsAny<CreateRoleServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.RoleController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequsetObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequsetObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsTheRole_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			RoleServiceModel roleServiceModel = new RoleServiceModel
			{
				Name = NAME
			};
			RoleWebModel roleWebModel = new RoleWebModel
			{
				Name = NAME
			};

			this.RoleServiceMock.Setup(p => p.GetRoleById(It.IsAny<Guid>())).Returns(Task.FromResult(roleServiceModel));
			this.MapperMock.Setup(p => p.Map<RoleWebModel>(It.IsAny<RoleServiceModel>())).Returns(roleWebModel);

			IActionResult result = this.RoleController.GetById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			RoleWebModel resultModel = okObjectResult.Value as Models.Identity.Role.RoleWebModel;

			Assert.AreEqual(NAME, resultModel.Name);
		}
		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenRoleIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdateRoleWebModel updateRoleWebModel = new UpdateRoleWebModel
			{
				Name = NAME
			};
			UpdateRoleServiceModel updateRoleServiceModel = new UpdateRoleServiceModel
			{
				Name = NAME
			};

			this.RoleServiceMock.Setup(p => p.UpdateRole(It.IsAny<UpdateRoleServiceModel>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateRoleServiceModel>(It.IsAny<UpdateRoleWebModel>())).Returns(updateRoleServiceModel);

			IActionResult result = this.RoleController.Update(id, updateRoleWebModel).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenRoleIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			string message = "Could not update role!";
			UpdateRoleWebModel updateRoleWebModel = new UpdateRoleWebModel
			{
				Name = NAME
			};
			UpdateRoleServiceModel updateRoleServiceModel = new UpdateRoleServiceModel
			{
				Name = NAME
			};

			this.RoleServiceMock.Setup(p => p.UpdateRole(It.IsAny<UpdateRoleServiceModel>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<UpdateRoleServiceModel>(It.IsAny<UpdateRoleWebModel>())).Returns(updateRoleServiceModel);

			IActionResult result = this.RoleController.Update(id, updateRoleWebModel).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenRoleIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this.RoleServiceMock.Setup(p => p.DeleteRole(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this.RoleController.Delete(id).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delet_ReturnsBadRequestObjectResult_WhenRoleIsNotDeletedSuccessfully()
		{
			string message = "Could not delete role!";
			Guid id = Guid.NewGuid();

			this.RoleServiceMock.Setup(p => p.DeleteRole(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this.RoleController.Delete(id).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
