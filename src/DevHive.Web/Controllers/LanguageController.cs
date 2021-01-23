using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Language;
using DevHive.Web.Models.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	[Authorize(Policy = "Administrator")]
	public class LanguageController
	{
		private readonly ILanguageService _languageService;
		private readonly IMapper _languageMapper;

		public LanguageController(ILanguageService languageService, IMapper mapper)
		{
			this._languageService = languageService;
			this._languageMapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateLanguageWebModel createLanguageWebModel)
		{
			CreateLanguageServiceModel languageServiceModel = this._languageMapper.Map<CreateLanguageServiceModel>(createLanguageWebModel);

			Guid id = await this._languageService.CreateLanguage(languageServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create language {createLanguageWebModel.Name}") :
				new OkObjectResult(new { Id = id });
		}

		[HttpGet]
		[Authorize(Policy = "User")]
		public async Task<IActionResult> GetById(Guid id)
		{
			ReadLanguageServiceModel languageServiceModel = await this._languageService.GetLanguageById(id);
			ReadLanguageWebModel languageWebModel = this._languageMapper.Map<ReadLanguageWebModel>(languageServiceModel);

			return new OkObjectResult(languageWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLanguageWebModel updateModel)
		{
			UpdateLanguageServiceModel updatelanguageServiceModel = this._languageMapper.Map<UpdateLanguageServiceModel>(updateModel);
			updatelanguageServiceModel.Id = id;

			bool result = await this._languageService.UpdateLanguage(updatelanguageServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update Language");

			return new OkResult();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._languageService.DeleteLanguage(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete Language");

			return new OkResult();
		}
	}
}
