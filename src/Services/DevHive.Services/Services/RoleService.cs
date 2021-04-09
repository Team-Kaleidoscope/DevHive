using System;
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Common.Constants;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Role;

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

		public async Task<Guid> CreateRole(CreateRoleServiceModel createRoleServiceModel)
		{
			if (await this._roleRepository.DoesNameExist(createRoleServiceModel.Name))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Role));

			Role role = this._roleMapper.Map<Role>(createRoleServiceModel);
			bool success = await this._roleRepository.AddAsync(role);

			if (success)
			{
				Role newRole = await this._roleRepository.GetByNameAsync(createRoleServiceModel.Name);
				return newRole.Id;
			}
			else
				return Guid.Empty;

		}

		public async Task<RoleServiceModel> GetRoleById(Guid id)
		{
			Role role = await this._roleRepository.GetByIdAsync(id) ??
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Role));

			return this._roleMapper.Map<RoleServiceModel>(role);
		}

		public async Task<bool> UpdateRole(UpdateRoleServiceModel updateRoleServiceModel)
		{
			if (!await this._roleRepository.DoesRoleExist(updateRoleServiceModel.Id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Role));

			if (await this._roleRepository.DoesNameExist(updateRoleServiceModel.Name))
				throw new DuplicateNameException(string.Format(ErrorMessages.AlreadyExists, ClassesConstants.Role));

			Role role = this._roleMapper.Map<Role>(updateRoleServiceModel);
			return await this._roleRepository.EditAsync(updateRoleServiceModel.Id, role);
		}

		public async Task<bool> DeleteRole(Guid id)
		{
			if (!await this._roleRepository.DoesRoleExist(id))
				throw new ArgumentNullException(string.Format(ErrorMessages.DoesNotExist, ClassesConstants.Role));

			Role role = await this._roleRepository.GetByIdAsync(id);
			return await this._roleRepository.DeleteAsync(role);
		}
	}
}
