using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Technology;
using DevHive.Services.Services;
using DevHive.Web.Models.Technology;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class TechnologyController
	{
		private readonly TechnologyService _technologyService;
		private readonly IMapper _technologyMapper;

		public TechnologyController(TechnologyService technologyService, IMapper mapper)
		{
			this._technologyService = technologyService;
			this._technologyMapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] TechnologyWebModel technologyWebModel)
		{
			TechnologyServiceModel technologyServiceModel = this._technologyMapper.Map<TechnologyServiceModel>(technologyWebModel);

			bool result = await this._technologyService.Create(technologyServiceModel);

			if(!result)
				return new BadRequestObjectResult("Could not create the Technology");

			return new OkResult();
		}

		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			TechnologyServiceModel technologyServiceModel = await this._technologyService.GetTechnologyById(id);
			TechnologyWebModel technologyWebModel = this._technologyMapper.Map<TechnologyWebModel>(technologyServiceModel);

			return new OkObjectResult(technologyWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTechnologyWebModel updateModel)
		{
			UpdateTechnologyServiceModel updateTechnologyWebModel = this._technologyMapper.Map<UpdateTechnologyServiceModel>(updateModel);
			updateTechnologyWebModel.Id = id;

			bool result = await this._technologyService.UpdateTechnology(updateTechnologyWebModel);

			if(!result)
				return new BadRequestObjectResult("Could not update Technology");

			return new OkResult();
		}
	
		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._technologyService.DeleteTechnology(id);
			
			if(!result)
				return new BadRequestObjectResult("Could not delete Technology");

			return new OkResult();
		}
	}
}