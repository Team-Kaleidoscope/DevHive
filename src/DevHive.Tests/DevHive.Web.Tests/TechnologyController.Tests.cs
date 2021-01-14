using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Technology;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class TechnologyControllerTests
	{
		private Mock<ITechnologyService> TechnologyServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private TechnologyController TechnologyController { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.TechnologyServiceMock = new Mock<ITechnologyService>();
			this.MapperMock = new Mock<IMapper>();
			this.TechnologyController = new TechnologyController(this.TechnologyServiceMock.Object, this.MapperMock.Object);
		}

		#region Create
		[Test]
		public void Create_ReturnsOkObjectResult_WhenTechnologyIsSuccessfullyCreated()
		{
			string name = "Gosho Trapov";

			CreateTechnologyWebModel createTechnologyWebModel = new CreateTechnologyWebModel
			{
				Name = name
			};
			CreateTechnologyServiceModel createTechnologyServiceModel = new CreateTechnologyServiceModel
			{
				Name = name
			};

			this.MapperMock.Setup(p => p.Map<CreateTechnologyServiceModel>(It.IsAny<CreateTechnologyWebModel>())).Returns(createTechnologyServiceModel);
			this.TechnologyServiceMock.Setup(p => p.Create(It.IsAny<CreateTechnologyServiceModel>())).Returns(Task.FromResult(true));

			IActionResult result = this.TechnologyController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}


		#endregion

	}
}
