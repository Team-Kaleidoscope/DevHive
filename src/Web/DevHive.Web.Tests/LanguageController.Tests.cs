using System;
using System.Linq;
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
		private Mock<ILanguageService> _languageServiceMock;
		private Mock<IMapper> _mapperMock;
		private LanguageController _languageController;

		[SetUp]
		public void SetUp()
		{
			this._languageServiceMock = new Mock<ILanguageService>();
			this._mapperMock = new Mock<IMapper>();
			this._languageController = new LanguageController(this._languageServiceMock.Object, this._mapperMock.Object);
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

			this._mapperMock
				.Setup(p => p.Map<CreateLanguageServiceModel>(It.IsAny<CreateLanguageWebModel>()))
				.Returns(createLanguageServiceModel);
			this._languageServiceMock
				.Setup(p => p.CreateLanguage(It.IsAny<CreateLanguageServiceModel>()))
				.ReturnsAsync(id);

			IActionResult result = this._languageController.Create(createLanguageWebModel).Result;

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

			this._mapperMock
				.Setup(p => p.Map<CreateLanguageServiceModel>(It.IsAny<CreateLanguageWebModel>()))
				.Returns(createTechnologyServiceModel);
			this._languageServiceMock
				.Setup(p => p.CreateLanguage(It.IsAny<CreateLanguageServiceModel>()))
				.ReturnsAsync(id);

			IActionResult result = this._languageController.Create(createTechnologyWebModel).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultMessage = badRequestObjectResult.Value.ToString();

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

			this._languageServiceMock
				.Setup(p => p.GetLanguageById(It.IsAny<Guid>()))
				.ReturnsAsync(readLanguageServiceModel);
			this._mapperMock
				.Setup(p => p.Map<ReadLanguageWebModel>(It.IsAny<ReadLanguageServiceModel>()))
				.Returns(readLanguageWebModel);

			IActionResult result = this._languageController.GetById(id).Result;

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

			this._languageServiceMock
				.Setup(p => p.UpdateLanguage(It.IsAny<UpdateLanguageServiceModel>()))
				.ReturnsAsync(true);
			this._mapperMock
				.Setup(p => p.Map<UpdateLanguageServiceModel>(It.IsAny<UpdateLanguageWebModel>()))
				.Returns(updateLanguageServiceModel);

			IActionResult result = this._languageController.Update(id, updateLanguageWebModel).Result;

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

			this._languageServiceMock
				.Setup(p => p.UpdateLanguage(It.IsAny<UpdateLanguageServiceModel>()))
				.ReturnsAsync(false);
			this._mapperMock
				.Setup(p => p.Map<UpdateLanguageServiceModel>(It.IsAny<UpdateLanguageWebModel>()))
				.Returns(updateLanguageServiceModel);

			IActionResult result = this._languageController.Update(id, updateLanguageWebModel).Result;
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

			this._languageServiceMock
				.Setup(p => p.DeleteLanguage(It.IsAny<Guid>()))
				.ReturnsAsync(true);

			IActionResult result = this._languageController.Delete(id).Result;

			Assert.IsInstanceOf<OkResult>(result);
		}

		[Test]
		public void Delet_ReturnsBadRequestObjectResult_WhenLanguageIsNotDeletedSuccessfully()
		{
			string message = "Could not delete Language";
			Guid id = Guid.NewGuid();

			this._languageServiceMock
				.Setup(p => p.DeleteLanguage(It.IsAny<Guid>()))
				.ReturnsAsync(false);

			IActionResult result = this._languageController.Delete(id).Result;

			Assert.IsInstanceOf<BadRequestObjectResult>(result);

			BadRequestObjectResult badRequestObjectResult = result as BadRequestObjectResult;
			string resultModel = badRequestObjectResult.Value.ToString();

			Assert.AreEqual(message, resultModel);
		}
		#endregion
	}
}
