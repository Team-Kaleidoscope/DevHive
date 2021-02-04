using AutoMapper;
using DevHive.Common.Models.Misc;
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
		private Mock<ITechnologyService> TechnologyServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private TechnologyController TechnologyController { get; set; }

		#region SetUp
		[SetUp]
		public void SetUp()
		{
			this.TechnologyServiceMock = new Mock<ITechnologyService>();
			this.MapperMock = new Mock<IMapper>();
			this.TechnologyController = new TechnologyController(this.TechnologyServiceMock.Object, this.MapperMock.Object);
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

			this.MapperMock.Setup(p => p.Map<CreateTechnologyServiceModel>(It.IsAny<CreateTechnologyWebModel>())).Returns(createTechnologyServiceModel);
			this.TechnologyServiceMock.Setup(p => p.CreateTechnology(It.IsAny<CreateTechnologyServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.TechnologyController.Create(createTechnologyWebModel).Result;

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

			this.MapperMock.Setup(p => p.Map<CreateTechnologyServiceModel>(It.IsAny<CreateTechnologyWebModel>())).Returns(createTechnologyServiceModel);
			this.TechnologyServiceMock.Setup(p => p.CreateTechnology(It.IsAny<CreateTechnologyServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.TechnologyController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequsetObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequsetObjectResult.Value.ToString();

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

			this.TechnologyServiceMock.Setup(p => p.GetTechnologyById(It.IsAny<Guid>())).Returns(Task.FromResult(readTechnologyServiceModel));
			this.MapperMock.Setup(p => p.Map<ReadTechnologyWebModel>(It.IsAny<ReadTechnologyServiceModel>())).Returns(readTechnologyWebModel);

			IActionResult result = this.TechnologyController.GetById(id).Result;

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

			this.TechnologyServiceMock.Setup(p => p.UpdateTechnology(It.IsAny<UpdateTechnologyServiceModel>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateTechnologyServiceModel>(It.IsAny<UpdateTechnologyWebModel>())).Returns(updateTechnologyServiceModel);

			IActionResult result = this.TechnologyController.Update(id, updateTechnologyWebModel).Result;

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

			this.TechnologyServiceMock.Setup(p => p.UpdateTechnology(It.IsAny<UpdateTechnologyServiceModel>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<UpdateTechnologyServiceModel>(It.IsAny<UpdateTechnologyWebModel>())).Returns(updateTechnologyServiceModel);

			IActionResult result = this.TechnologyController.Update(id, updateTechnologyWebModel).Result;
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

			this.TechnologyServiceMock.Setup(p => p.DeleteTechnology(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this.TechnologyController.Delete(id).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delet_ReturnsBadRequestObjectResult_WhenTechnologyIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Technology";
			Guid id = Guid.NewGuid();

			this.TechnologyServiceMock.Setup(p => p.DeleteTechnology(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this.TechnologyController.Delete(id).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
