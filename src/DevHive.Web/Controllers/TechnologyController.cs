using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;
using DevHive.Web.Models.Technology;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
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

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CreateTechnologyWebModel createTechnologyWebModel)
		{
			CreateTechnologyServiceModel technologyServiceModel = this._technologyMapper.Map<CreateTechnologyServiceModel>(createTechnologyWebModel);

			Guid id = await this._technologyService.Create(technologyServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create technology {createTechnologyWebModel.Name}") :
				new OkObjectResult(new { Id = id });
		}

		[HttpGet]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> GetById(Guid id)
		{
			CreateTechnologyServiceModel createTechnologyServiceModel = await this._technologyService.GetTechnologyById(id);
			CreateTechnologyWebModel createTechnologyWebModel = this._technologyMapper.Map<CreateTechnologyWebModel>(createTechnologyServiceModel);

			return new OkObjectResult(createTechnologyWebModel);
		}

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
