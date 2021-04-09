using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Language;
using DevHive.Web.Models.Language;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the language layer
	/// </summary>
	[ApiController]
	[Route("/api/[controller]")]
	public class LanguageController
	{
		private readonly ILanguageService _languageService;
		private readonly IMapper _languageMapper;

		public LanguageController(ILanguageService languageService, IMapper mapper)
		{
			this._languageService = languageService;
			this._languageMapper = mapper;
		}

		/// <summary>
		/// Create a new language, so users can have a choice. Admin only!
		/// </summary>
		/// <param name="createLanguageWebModel">The new language's parametars</param>
		/// <returns>The new language's Id</returns>
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CreateLanguageWebModel createLanguageWebModel)
		{
			CreateLanguageServiceModel languageServiceModel = this._languageMapper.Map<CreateLanguageServiceModel>(createLanguageWebModel);

			Guid id = await this._languageService.CreateLanguage(languageServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create language {createLanguageWebModel.Name}") :
				new OkObjectResult(new { Id = id });
		}

		/// <summary>
		/// Query full language data by Id
		/// </summary>
		/// <param name="id">The language's Id</param>
		/// <returns>Full language data</returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(Guid id)
		{
			ReadLanguageServiceModel languageServiceModel = await this._languageService.GetLanguageById(id);
			ReadLanguageWebModel languageWebModel = this._languageMapper.Map<ReadLanguageWebModel>(languageServiceModel);

			return new OkObjectResult(languageWebModel);
		}

		/// <summary>
		/// Query all languages in the database
		/// </summary>
		/// <returns>All languages in the database</returns>
		[HttpGet]
		[Route("GetLanguages")]
		[Authorize(Roles = "User,Admin")]
		public IActionResult GetAllExistingLanguages()
		{
			HashSet<ReadLanguageServiceModel> languageServiceModels = this._languageService.GetLanguages();
			HashSet<ReadLanguageWebModel> languageWebModels = this._languageMapper.Map<HashSet<ReadLanguageWebModel>>(languageServiceModels);

			return new OkObjectResult(languageWebModels);
		}

		/// <summary>
		/// Alter language's properties. Admin only!
		/// </summary>
		/// <param name="id">The language's Id</param>
		/// <param name="updateModel">The langauge's new parametars</param>
		/// <returns>Ok result</returns>
		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLanguageWebModel updateModel)
		{
			UpdateLanguageServiceModel updatelanguageServiceModel = this._languageMapper.Map<UpdateLanguageServiceModel>(updateModel);
			updatelanguageServiceModel.Id = id;

			bool result = await this._languageService.UpdateLanguage(updatelanguageServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update Language");

			return new OkResult();
		}

		/// <summary>
		/// Delete a language. Admin only!
		/// </summary>
		/// <param name="langaugeId">The language's Id</param>
		/// <returns>Ok result</returns>
		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid langaugeId)
		{
			bool result = await this._languageService.DeleteLanguage(langaugeId);

			if (!result)
				return new BadRequestObjectResult("Could not delete Language");

			return new OkResult();
		}
	}
}
