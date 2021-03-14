using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Role;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Role;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class RoleControllerTests
	{
		const string NAME = "Gosho Trapov";
		private Mock<IRoleService> _roleServiceMock;
		private Mock<IMapper> _mapperMock;
		private RoleController _roleController;

		[SetUp]
		public void SetUp()
		{
			this._roleServiceMock = new Mock<IRoleService>();
			this._mapperMock = new Mock<IMapper>();
			this._roleController = new RoleController(this._roleServiceMock.Object, this._mapperMock.Object);
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

			this._mapperMock.Setup(p => p.Map<CreateRoleServiceModel>(It.IsAny<CreateRoleWebModel>())).Returns(createRoleServiceModel);
			this._roleServiceMock.Setup(p => p.CreateRole(It.IsAny<CreateRoleServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this._roleController.Create(createRoleWebModel).Result;

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

			this._mapperMock.Setup(p => p.Map<CreateRoleServiceModel>(It.IsAny<CreateRoleWebModel>())).Returns(createTechnologyServiceModel);
			this._roleServiceMock.Setup(p => p.CreateRole(It.IsAny<CreateRoleServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this._roleController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequestObjectResult.Value.ToString();

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

			this._roleServiceMock.Setup(p => p.GetRoleById(It.IsAny<Guid>())).Returns(Task.FromResult(roleServiceModel));
			this._mapperMock.Setup(p => p.Map<RoleWebModel>(It.IsAny<RoleServiceModel>())).Returns(roleWebModel);

			IActionResult result = this._roleController.GetById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			RoleWebModel resultModel = okObjectResult.Value as RoleWebModel;

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

			this._roleServiceMock.Setup(p => p.UpdateRole(It.IsAny<UpdateRoleServiceModel>())).Returns(Task.FromResult(true));
			this._mapperMock.Setup(p => p.Map<UpdateRoleServiceModel>(It.IsAny<UpdateRoleWebModel>())).Returns(updateRoleServiceModel);

			IActionResult result = this._roleController.Update(id, updateRoleWebModel).Result;

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

			this._roleServiceMock.Setup(p => p.UpdateRole(It.IsAny<UpdateRoleServiceModel>())).Returns(Task.FromResult(false));
			this._mapperMock.Setup(p => p.Map<UpdateRoleServiceModel>(It.IsAny<UpdateRoleWebModel>())).Returns(updateRoleServiceModel);

			IActionResult result = this._roleController.Update(id, updateRoleWebModel).Result;
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

			this._roleServiceMock.Setup(p => p.DeleteRole(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this._roleController.Delete(id).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delet_ReturnsBadRequestObjectResult_WhenRoleIsNotDeletedSuccessfully()
		{
			string message = "Could not delete role!";
			Guid id = Guid.NewGuid();

			this._roleServiceMock.Setup(p => p.DeleteRole(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this._roleController.Delete(id).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
