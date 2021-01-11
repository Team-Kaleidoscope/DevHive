using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Models.Identity;
using DevHive.Data.Models;
using DevHive.Data.Repositories.Contracts;

namespace DevHive.Services.Services
{
	public class RoleService
	{
		private readonly IRoleRepository _roleRepository;
		private readonly IMapper _roleMapper;

		public RoleService(IRoleRepository roleRepository, IMapper mapper)
		{
			this._roleRepository = roleRepository;
			this._roleMapper = mapper;
		}

		public async Task<bool> CreateRole(RoleModel roleServiceModel)
		{
			if (await this._roleRepository.DoesNameExist(roleServiceModel.Name))
				throw new ArgumentException("Role already exists!");

			Role role = this._roleMapper.Map<Role>(roleServiceModel);

			return await this._roleRepository.AddAsync(role);
		}

		public async Task<RoleModel> GetRoleById(Guid id)
		{
			Role role = await this._roleRepository.GetByIdAsync(id) 
				?? throw new ArgumentException("Role does not exist!");

			return this._roleMapper.Map<RoleModel>(role);
		}

		public async Task<bool> UpdateRole(RoleModel roleServiceModel)
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
