using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevHive.Web.Models.Role;
using AutoMapper;
using System;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Role;
using Microsoft.AspNetCore.Authorization;

namespace DevHive.Web.Controllers
{
	/// <summary>
	/// All endpoints for interacting with the roles layer
	/// </summary>
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

		/// <summary>
		/// Create a new role for the roles hierarchy. Admin only!
		/// </summary>
		/// <param name="createRoleWebModel">The new role's parametars</param>
		/// <returns>The new role's Id</returns>
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CreateRoleWebModel createRoleWebModel)
		{
			CreateRoleServiceModel roleServiceModel =
				this._roleMapper.Map<CreateRoleServiceModel>(createRoleWebModel);

			Guid id = await this._roleService.CreateRole(roleServiceModel);

			return id == Guid.Empty ?
				new BadRequestObjectResult($"Could not create role {createRoleWebModel.Name}") :
				new OkObjectResult(new { Id = id });
		}

		/// <summary>
		/// Get a role's full data, querying it by it's Id
		/// </summary>
		/// <param name="id">The role's Id</param>
		/// <returns>Full info of the role</returns>
		[HttpGet]
		[Authorize(Roles = "User,Admin")]
		public async Task<IActionResult> GetById(Guid id)
		{
			RoleServiceModel roleServiceModel = await this._roleService.GetRoleById(id);
			RoleWebModel roleWebModel = this._roleMapper.Map<RoleWebModel>(roleServiceModel);

			return new OkObjectResult(roleWebModel);
		}

		/// <summary>
		/// Update a role's parametars. Admin only!
		/// </summary>
		/// <param name="id">The role's Id</param>
		/// <param name="updateRoleWebModel">The new parametrats for that role</param>
		/// <returns>Ok result</returns>
		[HttpPut]
		[Authorize(Roles = "Admin")]
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

		/// <summary>
		/// Delete a role. Admin only!
		/// </summary>
		/// <param name="id">The role's Id</param>
		/// <returns>Ok result</returns>
		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			bool result = await this._roleService.DeleteRole(id);

			if (!result)
				return new BadRequestObjectResult("Could not delete role!");

			return new OkResult();
		}
	}
}
