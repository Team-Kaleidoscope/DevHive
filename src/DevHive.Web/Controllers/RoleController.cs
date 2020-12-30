using System.Threading.Tasks;
using DevHive.Data.Repositories;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Mvc;
using DevHive.Web.Models.Identity.Role;
using AutoMapper;
using System;
using DevHive.Common.Models.Identity;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	//[Authorize(Roles = "Admin")]
	public class RoleController
	{
		private readonly RoleService _roleService;
		private readonly IMapper _roleMapper;

		public RoleController(RoleService roleService, IMapper mapper)
		{
			this._roleService = roleService;
			this._roleMapper = mapper;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateRoleModel createRoleModel)
		{
			RoleModel roleServiceModel = 
				this._roleMapper.Map<RoleModel>(createRoleModel); 
			
			bool result = await this._roleService.CreateRole(roleServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not create role!");

			return new OkResult();
		}

		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			RoleModel roleServiceModel = await this._roleService.GetRoleById(id);
			RoleModel roleWebModel = this._roleMapper.Map<RoleModel>(roleServiceModel);

			return new OkObjectResult(roleWebModel);
		}

		[HttpPut]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleModel updateRoleModel)
		{
			RoleModel roleServiceModel = 
				this._roleMapper.Map<RoleModel>(updateRoleModel);
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
				return new BadRequestObjectResult("Could not delete role!");

			return new OkResult();
		}
	}
}
