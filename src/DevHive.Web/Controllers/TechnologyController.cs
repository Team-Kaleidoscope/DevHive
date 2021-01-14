using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Technology;
using DevHive.Web.Models.Technology;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
    [ApiController]
	[Route("/api/[controller]")]
	public class TechnologyController
	{
		private readonly ITechnologyService _technologyService;
		private readonly IMapper _technologyMapper;

		public TechnologyController(ITechnologyService technologyService, IMapper mapper)
		{
			this._technologyService = technologyService;
			this._technologyMapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateTechnologyWebModel technologyWebModel)
		{
			CreateTechnologyServiceModel technologyServiceModel = this._technologyMapper.Map<CreateTechnologyServiceModel>(technologyWebModel);

			bool result = await this._technologyService.Create(technologyServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not create the Technology");

			return new OkResult();
		}

		[HttpGet]
		public async Task<IActionResult> GetById(Guid technologyId)
		{
			TechnologyServiceModel technologyServiceModel = await this._technologyService.GetTechnologyById(technologyId);
			TechnologyWebModel technologyWebModel = this._technologyMapper.Map<TechnologyWebModel>(technologyServiceModel);

			return new OkObjectResult(technologyWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Guid technologyId, [FromBody] UpdateTechnologyWebModel updateModel)
		{
			UpdateTechnologyServiceModel updateTechnologyWebModel = this._technologyMapper.Map<UpdateTechnologyServiceModel>(updateModel);

			bool result = await this._technologyService.UpdateTechnology(technologyId, updateTechnologyWebModel);

			if (!result)
				return new BadRequestObjectResult("Could not update Technology");

			return new OkResult();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Guid technologyId)
		{
			bool result = await this._technologyService.DeleteTechnology(technologyId);

			if (!result)
				return new BadRequestObjectResult("Could not delete Technology");

			return new OkResult();
		}
	}
}
