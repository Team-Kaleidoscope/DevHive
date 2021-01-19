using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevHive.Web.Models.Identity.Role;
using AutoMapper;
using System;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Identity.Role;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	//[Authorize(Roles = "Admin")]
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
		public async Task<IActionResult> Create([FromBody] CreateRoleWebModel createRoleWebModel)
		{
			RoleServiceModel roleServiceModel =
				this._roleMapper.Map<RoleServiceModel>(createRoleWebModel);

			Guid id = await this._roleService.CreateRole(roleServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create role {createRoleWebModel.Name}") :
				new OkObjectResult(new { Id = id });

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
				return new BadRequestObjectResult("Could not delete role!");

			return new OkResult();
		}
	}
}
