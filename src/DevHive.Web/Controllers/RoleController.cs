using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevHive.Web.Models.Identity.Role;
using AutoMapper;
using System;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Identity.Role;
using Microsoft.AspNetCore.Authorization;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class RoleController
	{
		private readonly IRoleService _roleService;
		private readonly IMapper _roleMapper;

		public RoleController(IRoleService roleService, IMapper mapper)
		{
			this._roleService = roleService;
			this._roleMapper = mapper;
		}

		[HttpPost]
		[Authorize(Policy = "Administrator")]
		public async Task<IActionResult> Create([FromBody] CreateRoleWebModel createRoleWebModel)
		{
			CreateRoleServiceModel roleServiceModel =
				this._roleMapper.Map<CreateRoleServiceModel>(createRoleWebModel);

			Guid id = await this._roleService.CreateRole(roleServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create role {createRoleWebModel.Name}") :
				new OkObjectResult(new { Id = id });
		}

		[HttpGet]
		[Authorize(Policy = "User")]
		public async Task<IActionResult> GetById(Guid id)
		{
			RoleServiceModel roleServiceModel = await this._roleService.GetRoleById(id);
			RoleWebModel roleWebModel = this._roleMapper.Map<RoleWebModel>(roleServiceModel);

			return new OkObjectResult(roleWebModel);
		}

		[HttpPut]
		[Authorize(Policy = "Administrator")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleWebModel updateRoleWebModel)
		{
			UpdateRoleServiceModel updateRoleServiceModel =
				this._roleMapper.Map<UpdateRoleServiceModel>(updateRoleWebModel);
			updateRoleServiceModel.Id = id;

			bool result = await this._roleService.UpdateRole(updateRoleServiceModel);

			if (!result)
				return new BadRequestObjectResult("Could not update role!");

			return new OkResult();
		}

		[HttpDelete]
		[Authorize(Policy = "Administrator")]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._roleService.DeleteRole(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete role!");

			return new OkResult();
		}
	}
}
