using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Identity.Role;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Services.Services
{
	public class RoleService
	{
		private readonly RoleRepository _roleRepository;
		private readonly IMapper _roleMapper;

		public RoleService(DevHiveContext context, IMapper mapper)
		{
			this._roleRepository = new RoleRepository(context);
			this._roleMapper = mapper;
		}

		public async Task<IActionResult> CreateRole(RoleServiceModel roleServiceModel)
		{
			if (!await this._roleRepository.DoesNameExist(roleServiceModel.Name))
				return new BadRequestObjectResult("Invalid role name!");

			Role role = this._roleMapper.Map<Role>(roleServiceModel);

			await this._roleRepository.AddAsync(role);

			return new CreatedResult("CreateRole", role);
		}

		public async Task<IActionResult> GetRoleById(Guid id)
		{
			Role role = await this._roleRepository.GetByIdAsync(id);

			if (role == null)
				return new NotFoundObjectResult("Role does not exist!");

			return new ObjectResult(role);
		}

		public async Task<IActionResult> UpdateRole(RoleServiceModel roleServiceModel)
		{
			if (!await this._roleRepository.DoesRoleExist(roleServiceModel.Id))
				return new NotFoundObjectResult("Role does not exist!");

			if (!await this._roleRepository.DoesNameExist(roleServiceModel.Name))
				return new BadRequestObjectResult("Role name already exists!");

			Role role = this._roleMapper.Map<Role>(roleServiceModel);
			await this._roleRepository.EditAsync(role);

			return new AcceptedResult("UpdateRole", role);
		}

		public async Task<IActionResult> DeleteRole(Guid id)
		{
			if (!await this._roleRepository.DoesRoleExist(id))
				return new NotFoundObjectResult("Role does not exist!");

			Role role = await this._roleRepository.GetByIdAsync(id);
			await this._roleRepository.DeleteAsync(role);

			return new OkResult();
		}
	}
}
