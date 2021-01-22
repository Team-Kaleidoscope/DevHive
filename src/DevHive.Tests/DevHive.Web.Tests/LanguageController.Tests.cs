using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Language;
using DevHive.Web.Controllers;
using DevHive.Web.Models.Language;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DevHive.Web.Tests
{
	[TestFixture]
	public class LanguageControllerTests
	{
		const string NAME = "Gosho Trapov";
		private Mock<ILanguageService> LanguageServiceMock { get; set; }
		private Mock<IMapper> MapperMock { get; set; }
		private LanguageController LanguageController { get; set; }

		[SetUp]
		public void SetUp()
		{
			this.LanguageServiceMock = new Mock<ILanguageService>();
			this.MapperMock = new Mock<IMapper>();
			this.LanguageController = new LanguageController(this.LanguageServiceMock.Object, this.MapperMock.Object);
		}

		#region Create
		[Test]
		public void CreateLanguage_ReturnsOkObjectResult_WhenLanguageIsSuccessfullyCreated()
		{
			CreateLanguageWebModel createLanguageWebModel = new CreateLanguageWebModel
			{
				Name = NAME
			};
			CreateLanguageServiceModel createLanguageServiceModel = new CreateLanguageServiceModel
			{
				Name = NAME
			};
			Guid id = Guid.NewGuid();

			this.MapperMock.Setup(p => p.Map<CreateLanguageServiceModel>(It.IsAny<CreateLanguageWebModel>())).Returns(createLanguageServiceModel);
			this.LanguageServiceMock.Setup(p => p.CreateLanguage(It.IsAny<CreateLanguageServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.LanguageController.Create(createLanguageWebModel).Result;

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
		public void CreateLanguage_ReturnsBadRequestObjectResult_WhenLanguageIsNotCreatedSuccessfully()
		{
			CreateLanguageWebModel createTechnologyWebModel = new CreateLanguageWebModel
			{
				Name = NAME
			};
			CreateLanguageServiceModel createTechnologyServiceModel = new CreateLanguageServiceModel
			{
				Name = NAME
			};
			Guid id = Guid.Empty;
			string errorMessage = $"Could not create language {NAME}";

			this.MapperMock.Setup(p => p.Map<CreateLanguageServiceModel>(It.IsAny<CreateLanguageWebModel>())).Returns(createTechnologyServiceModel);
			this.LanguageServiceMock.Setup(p => p.CreateLanguage(It.IsAny<CreateLanguageServiceModel>())).Returns(Task.FromResult(id));

			IActionResult result = this.LanguageController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequsetObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequsetObjectResult.Value.ToString();

			Assert.AreEqual(errorMessage, resultMessage);
		}
		#endregion

		#region Read
		[Test]
		public void GetById_ReturnsTheLanguage_WhenItExists()
		{
			Guid id = Guid.NewGuid();

			ReadLanguageServiceModel readLanguageServiceModel = new ReadLanguageServiceModel
			{
				Name = NAME
			};
			ReadLanguageWebModel readLanguageWebModel = new ReadLanguageWebModel
			{
				Name = NAME
			};

			this.LanguageServiceMock.Setup(p => p.GetLanguageById(It.IsAny<Guid>())).Returns(Task.FromResult(readLanguageServiceModel));
			this.MapperMock.Setup(p => p.Map<ReadLanguageWebModel>(It.IsAny<ReadLanguageServiceModel>())).Returns(readLanguageWebModel);

			IActionResult result = this.LanguageController.GetById(id).Result;

			Assert.IsInstanceOf<OkObjectResult>(result);

			OkObjectResult okObjectResult = result as OkObjectResult;
			ReadLanguageWebModel resultModel = okObjectResult.Value as Models.Language.ReadLanguageWebModel;

			Assert.AreEqual(NAME, resultModel.Name);
		}
		#endregion

		#region Update
		[Test]
		public void Update_ShouldReturnOkResult_WhenLanguageIsUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			UpdateLanguageWebModel updateLanguageWebModel = new UpdateLanguageWebModel
			{
				Name = NAME
			};
			UpdateLanguageServiceModel updateLanguageServiceModel = new UpdateLanguageServiceModel
			{
				Name = NAME
			};

			this.LanguageServiceMock.Setup(p => p.UpdateLanguage(It.IsAny<UpdateLanguageServiceModel>())).Returns(Task.FromResult(true));
			this.MapperMock.Setup(p => p.Map<UpdateLanguageServiceModel>(It.IsAny<UpdateLanguageWebModel>())).Returns(updateLanguageServiceModel);

			IActionResult result = this.LanguageController.Update(id, updateLanguageWebModel).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Update_ShouldReturnBadObjectResult_WhenLanguageIsNotUpdatedSuccessfully()
		{
			Guid id = Guid.NewGuid();
			string message = "Could not update Language";
			UpdateLanguageWebModel updateLanguageWebModel = new UpdateLanguageWebModel
			{
				Name = NAME
			};
			UpdateLanguageServiceModel updateLanguageServiceModel = new UpdateLanguageServiceModel
			{
				Name = NAME
			};

			this.LanguageServiceMock.Setup(p => p.UpdateLanguage(It.IsAny<UpdateLanguageServiceModel>())).Returns(Task.FromResult(false));
			this.MapperMock.Setup(p => p.Map<UpdateLanguageServiceModel>(It.IsAny<UpdateLanguageWebModel>())).Returns(updateLanguageServiceModel);

			IActionResult result = this.LanguageController.Update(id, updateLanguageWebModel).Result;
			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion

		#region Delete
		[Test]
		public void Delete_ReturnsOkResult_WhenLanguageIsDeletedSuccessfully()
		{
			Guid id = Guid.NewGuid();

			this.LanguageServiceMock.Setup(p => p.DeleteLanguage(It.IsAny<Guid>())).Returns(Task.FromResult(true));

			IActionResult result = this.LanguageController.Delete(id).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delet_ReturnsBadRequestObjectResult_WhenLanguageIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Language";
			Guid id = Guid.NewGuid();

			this.LanguageServiceMock.Setup(p => p.DeleteLanguage(It.IsAny<Guid>())).Returns(Task.FromResult(false));

			IActionResult result = this.LanguageController.Delete(id).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
