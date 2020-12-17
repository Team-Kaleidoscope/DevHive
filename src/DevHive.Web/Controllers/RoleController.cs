using System.Threading.Tasks;
using DevHive.Data.Repositories;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Mvc;
using DevHive.Web.Models.Identity.Role;
using AutoMapper;
using DevHive.Services.Models.Identity.Role;
using System;
using Microsoft.AspNetCore.Authorization;

namespace DevHive.Web.Controllers
{
    [ApiController]
	[Route("/api/[controller]")]
	//[Authorize(Roles = "Admin")]
	public class RoleController
	{
		private readonly RoleService _roleService;
		private readonly IMapper _roleMapper;

		public RoleController(DevHiveContext context, IMapper mapper)
		{
			this._roleService = new RoleService(context, mapper);
			this._roleMapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateRoleWebModel createRoleWebModel)
		{
			RoleServiceModel roleServiceModel = 
				this._roleMapper.Map<RoleServiceModel>(createRoleWebModel); 
			
			bool result = await this._roleService.CreateRole(roleServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not create role!");

			return new OkResult();
		}

		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			RoleServiceModel roleServiceModel = await this._roleService.GetRoleById(id);
			RoleWebModel roleWebModel = this._roleMapper.Map<RoleWebModel>(roleServiceModel);

			return new OkObjectResult(roleWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleWebModel updateRoleWebModel)
		{
			RoleServiceModel roleServiceModel = 
				this._roleMapper.Map<RoleServiceModel>(updateRoleWebModel);
			roleServiceModel.Id = id;

			bool result = await this._roleService.UpdateRole(roleServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update role!");

			return new OkResult();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._roleService.DeleteRole(id);

			if (!result)
				return new BadRequestObjectResult("Could nor delete role!");

			return new OkResult();
		}
	}
}
