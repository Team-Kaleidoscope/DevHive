using System.Net;
using System.Threading.Tasks;
using API.Database;
using AutoMapper;
using Data.Models.Classes;
using Data.Models.DTOs;
using Newtonsoft.Json;
using System.Web.Http;
using System.Net.Http;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using System;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

namespace API.Service
{
	public class UserService
	{
		private readonly UserDbRepository _userDbRepository;
		private readonly IMapper _userMapper;

		public UserService(DevHiveContext context, IMapper mapper)
		{
			this._userDbRepository = new UserDbRepository(context);
			this._userMapper = mapper;
		}
	
		public async Task<IActionResult> CreateUser(UserDTO userDTO)
		{
			if (this._userDbRepository.DoesUsernameExist(userDTO.UserName))
				return new BadRequestObjectResult("Username already exists!");

			User user = this._userMapper.Map<User>(userDTO);
			await this._userDbRepository.AddAsync(user);

			return new OkObjectResult("User created.");
		}

		public async Task<IActionResult> GetUserById(int id) 
		{
			User user = await this._userDbRepository.FindByIdAsync(id);

			if (user == null)
				return new NotFoundObjectResult("User does not exist!");

			return new OkObjectResult(user);
		}

		public async Task<IActionResult> UpdateUser(int id, UserDTO userDTO)
		{
			if (!this._userDbRepository.DoesUserExist(id))
				return new NotFoundObjectResult("User does not exist!");

			if (this._userDbRepository.DoesUsernameExist(userDTO.UserName))
				return new BadRequestObjectResult("Username already exists!");

			User user = this._userMapper.Map<User>(userDTO);
			await this._userDbRepository.EditAsync(id, user);

			return new OkObjectResult("User updated.");
		}

		public async Task<IActionResult> DeleteUser(int id)
		{
			if (!this._userDbRepository.DoesUserExist(id))
				return new NotFoundObjectResult("User does not exist!");

			await this._userDbRepository.DeleteAsync(id);
			
			return new OkObjectResult("User deleted successfully.");
		}
	}
}
