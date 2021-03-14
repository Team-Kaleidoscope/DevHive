using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Technology;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class TechnologyControllerTests
	{
		const string NAME = "Gosho Trapov";
		private Mock<ITechnologyService> _technologyServiceMock;
		private Mock<IMapper> _mapperMock;
		private TechnologyController _technologyController;

		#region SetUp
		[SetUp]
		public void SetUp()
		{
			this._technologyServiceMock = new Mock<ITechnologyService>();
			this._mapperMock = new Mock<IMapper>();
			this._technologyController = new TechnologyController(this._technologyServiceMock.Object, this._mapperMock.Object);
		}
		#endregion

		#region Create
		[Test]
		public void Create_ReturnsOkObjectResult_WhenTechnologyIsSuccessfullyCreated()
		{
			CreateTechnologyWebModel createTechnologyWebModel = new CreateTechnologyWebModel
			{
				Name = NAME
			};
			CreateTechnologyServiceModel createTechnologyServiceModel = new CreateTechnologyServiceModel
			{
				Name = NAME
			};
			Guid id = Guid.NewGuid();

			this._mapperMock.Setup(p => p.Map<CreateTechnologyServiceModel>(It.IsAny<CreateTechnologyWebModel>())).Returns(createTechnologyServiceModel);
			this._technologyServiceMock.Setup(p => p.CreateTechnology(It.IsAny<CreateTechnologyServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this._technologyController.Create(createTechnologyWebModel).Result;

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
		public void Create_ReturnsBadRequestObjectResult_WhenTechnologyIsNotCreatedSuccessfully()
		{
			CreateTechnologyWebModel createTechnologyWebModel = new CreateTechnologyWebModel
			{
				Name = NAME
			};
			CreateTechnologyServiceModel createTechnologyServiceModel = new CreateTechnologyServiceModel
			{
				Name = NAME
			};
			Guid id = Guid.Empty;
			string errorMessage = $"Could not create technology {NAME}";

			this._mapperMock.Setup(p => p.Map<CreateTechnologyServiceModel>(It.IsAny<CreateTechnologyWebModel>())).Returns(createTechnologyServiceModel);
			this._technologyServiceMock.Setup(p => p.CreateTechnology(It.IsAny<CreateTechnologyServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this._technologyController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsTheThecnology_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			ReadTechnologyWebModel readTechnologyWebModel = new ReadTechnologyWebModel
			{
				Name = NAME
			};
			ReadTechnologyServiceModel readTechnologyServiceModel = new ReadTechnologyServiceModel
			{
				Name = NAME
			};

			this._technologyServiceMock.Setup(p => p.GetTechnologyById(It.IsAny<Guid>())).Returns(Task.FromResult(readTechnologyServiceModel));
			this._mapperMock.Setup(p => p.Map<ReadTechnologyWebModel>(It.IsAny<ReadTechnologyServiceModel>())).Returns(readTechnologyWebModel);

			IActionResult result = this._technologyController.GetById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadTechnologyWebModel resultModel = okObjectResult.Value as Models.Technology.ReadTechnologyWebModel;

			Assert.AreEqual(NAME, resultModel.Name);
		}
		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenTechnologyIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdateTechnologyWebModel updateTechnologyWebModel = new UpdateTechnologyWebModel
			{
				Name = NAME
			};
			UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
			{
				Name = NAME
			};

			this._technologyServiceMock.Setup(p => p.UpdateTechnology(It.IsAny<UpdateTechnologyServiceModel>())).Returns(Task.FromResult(true));
			this._mapperMock.Setup(p => p.Map<UpdateTechnologyServiceModel>(It.IsAny<UpdateTechnologyWebModel>())).Returns(updateTechnologyServiceModel);

			IActionResult result = this._technologyController.Update(id, updateTechnologyWebModel).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenTechnologyIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			string message = "Could not update Technology";
			UpdateTechnologyWebModel updateTechnologyWebModel = new UpdateTechnologyWebModel
			{
				Name = NAME
			};
			UpdateTechnologyServiceModel updateTechnologyServiceModel = new UpdateTechnologyServiceModel
			{
				Name = NAME
			};

			this._technologyServiceMock.Setup(p => p.UpdateTechnology(It.IsAny<UpdateTechnologyServiceModel>())).Returns(Task.FromResult(false));
			this._mapperMock.Setup(p => p.Map<UpdateTechnologyServiceModel>(It.IsAny<UpdateTechnologyWebModel>())).Returns(updateTechnologyServiceModel);

			IActionResult result = this._technologyController.Update(id, updateTechnologyWebModel).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenTechnologyIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this._technologyServiceMock.Setup(p => p.DeleteTechnology(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this._technologyController.Delete(id).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delet_ReturnsBadRequestObjectResult_WhenTechnologyIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Technology";
			Guid id = Guid.NewGuid();

			this._technologyServiceMock.Setup(p => p.DeleteTechnology(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this._technologyController.Delete(id).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
