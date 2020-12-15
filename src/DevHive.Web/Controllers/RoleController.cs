using System.Threading.Tasks;
using DevHive.Data.Repositories;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Mvc;
using DevHive.Web.Models.Identity.Role;
using AutoMapper;
using DevHive.Services.Models.Identity.Role;
using System;

namespace DevHive.Web.Controllers
{
    [ApiController]
	[Route("/api/[controller]")]
	public class RoleController
	{
		private readonly RoleService _service;
		private readonly IMapper _roleMapper;

		public RoleController(DevHiveContext context, IMapper mapper)
		{
			this._service = new RoleService(context, mapper);
		}

		[HttpPost]
		public Task<IActionResult> Create(CreateRoleWebModel createRoleWebModel)
		{
			RoleServiceModel roleServiceModel = 
				this._roleMapper.Map<RoleServiceModel>(createRoleWebModel); 
			
			return this._service.CreateRole(roleServiceModel);
		}

		[HttpGet]
		public Task<IActionResult> Get(Guid id)
		{
			return this._service.GetRoleById(id);
		}

		[HttpPut]
		public Task<IActionResult> Update(UpdateRoleWebModel updateRoleWebModel)
		{
			RoleServiceModel roleServiceModel = 
				this._roleMapper.Map<RoleServiceModel>(updateRoleWebModel);

			return this._service.UpdateRole(roleServiceModel);
		}

		[HttpDelete]
		public Task<IActionResult> Delete(Guid id)
		{
			return this._service.DeleteRole(id);
		}
	}
}
