using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Identity.Role;

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

		public async Task<bool> CreateRole(RoleServiceModel roleServiceModel)
		{
			if (await this._roleRepository.DoesNameExist(roleServiceModel.Name))
				throw new ArgumentException("Role already exists!");

			Role role = this._roleMapper.Map<Role>(roleServiceModel);

			return await this._roleRepository.AddAsync(role);
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
