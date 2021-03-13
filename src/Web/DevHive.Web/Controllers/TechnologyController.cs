using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;
using DevHive.Web.Models.Technology;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the technology layer
	/// </summary>
	[ApiController]
	[Route("/api/[controller]")]
	public class TechnologyController
	{
		private readonly ITechnologyService _technologyService;
		private readonly IMapper _technologyMapper;

		public TechnologyController(ITechnologyService technologyService, IMapper technologyMapper)
		{
			this._technologyService = technologyService;
			this._technologyMapper = technologyMapper;
		}

		/// <summary>
		/// Create a new technology, so users can have a choice. Admin only!
		/// </summary>
		/// <param name="createTechnologyWebModel">Data for the new technology</param>
		/// <returns>The new technology's Id</returns>
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CreateTechnologyWebModel createTechnologyWebModel)
		{
			CreateTechnologyServiceModel technologyServiceModel = this._technologyMapper.Map<CreateTechnologyServiceModel>(createTechnologyWebModel);

			Guid id = await this._technologyService.CreateTechnology(technologyServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create technology {createTechnologyWebModel.Name}") :
				new OkObjectResult(new { Id = id });
		}

		/// <summary>
		/// Get technology's data by it's Id
		/// </summary>
		/// <param name="id">The technology's Id</param>
		/// <returns>The technology's full data</returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> GetById(Guid id)
		{
			ReadTechnologyServiceModel readTechnologyServiceModel = await this._technologyService.GetTechnologyById(id);
			ReadTechnologyWebModel readTechnologyWebModel = this._technologyMapper.Map<ReadTechnologyWebModel>(readTechnologyServiceModel);

			return new OkObjectResult(readTechnologyWebModel);
		}

		/// <summary>
		/// Get all technologies from our database
		/// </summary>
		/// <returns>All technologies</returns>
		[HttpGet]
		[Route("GetTechnologies")]
		[Authorize(Roles = "User,Admin")]
		public IActionResult GetAllExistingTechnologies()
		{
			HashSet<ReadTechnologyServiceModel> technologyServiceModels = this._technologyService.GetTechnologies();
			HashSet<ReadTechnologyWebModel> languageWebModels = this._technologyMapper.Map<HashSet<ReadTechnologyWebModel>>(technologyServiceModels);

			return new OkObjectResult(languageWebModels);
		}

		/// <summary>
		/// Alter a technology's parameters. Admin only!
		/// </summary>
		/// <param name="id">Technology's Id</param>
		/// <param name="updateModel">The new parametars</param>
		/// <returns>Ok result</returns>
		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTechnologyWebModel updateModel)
		{
			UpdateTechnologyServiceModel updateTechnologyServiceModel = this._technologyMapper.Map<UpdateTechnologyServiceModel>(updateModel);
			updateTechnologyServiceModel.Id = id;

			bool result = await this._technologyService.UpdateTechnology(updateTechnologyServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update Technology");

			return new OkResult();
		}

		/// <summary>
		/// Delete a etchnology from the database. Admin only!
		/// </summary>
		/// <param name="id">The technology's Id</param>
		/// <returns>Ok result</returns>
		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._technologyService.DeleteTechnology(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete Technology");

			return new OkResult();
		}
	}
}
