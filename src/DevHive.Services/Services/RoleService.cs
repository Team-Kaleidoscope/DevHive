using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Identity.Role;
using DevHive.Services.Models.Language;

namespace DevHive.Services.Services
{
    public class RoleService : IRoleService
	{
		private readonly IRoleRepository _roleRepository;
		private readonly IMapper _roleMapper;

		public RoleService(IRoleRepository roleRepository, IMapper mapper)
		{
			this._roleRepository = roleRepository;
			this._roleMapper = mapper;
		}

		public async Task<Guid> CreateRole(RoleServiceModel roleServiceModel)
		{
			if (await this._roleRepository.DoesNameExist(roleServiceModel.Name))
				throw new ArgumentException("Role already exists!");


			Role role = this._roleMapper.Map<Role>(roleServiceModel);
			bool success = await this._roleRepository.AddAsync(role);

			if(success)
			{
				Role newRole = await this._roleRepository.GetByNameAsync(roleServiceModel.Name);
				return newRole.Id;
			}
			else
				return Guid.Empty;

		}

		public async Task<RoleServiceModel> GetRoleById(Guid id)
		{
			Role role = await this._roleRepository.GetByIdAsync(id)
				?? throw new ArgumentException("Role does not exist!");

			return this._roleMapper.Map<RoleServiceModel>(role);
		}

		public async Task<bool> UpdateRole(RoleServiceModel roleServiceModel)
		{
			if (!await this._roleRepository.DoesRoleExist(roleServiceModel.Id))
				throw new ArgumentException("Role does not exist!");

			if (await this._roleRepository.DoesNameExist(roleServiceModel.Name))
				throw new ArgumentException("Role name already exists!");

			Role role = this._roleMapper.Map<Role>(roleServiceModel);
			return await this._roleRepository.EditAsync(role);
		}

		public async Task<bool> DeleteRole(Guid id)
		{
			if (!await this._roleRepository.DoesRoleExist(id))
				throw new ArgumentException("Role does not exist!");

			Role role = await this._roleRepository.GetByIdAsync(id);
			return await this._roleRepository.DeleteAsync(role);
		}
	}
}
