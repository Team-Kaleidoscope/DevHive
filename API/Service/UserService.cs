using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Database;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Classes;
using Models.DTOs;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Collections.Generic;

namespace API.Service
{
	public class UserService
	{
		private readonly DbRepository<User> _dbRepository;
		private readonly IMapper _userMapper;

		public UserService(DevHiveContext context, IMapper mapper)
		{
			this._dbRepository = new DbRepository<User>(context);
			this._userMapper = mapper;
		}
	
		public async Task<HttpStatusCode> CreateUser(UserDTO userDTO)
		{
			IEnumerable<User> allUsers = this._dbRepository.Query();

			foreach (var currUser in allUsers)
			{
				if (currUser.UserName == userDTO.UserName)
					return HttpStatusCode.Forbidden;
			}

			User user = this._userMapper.Map<User>(userDTO);
			await this._dbRepository.AddAsync(user);

			return HttpStatusCode.OK;
		}

		public async Task<string> GetUserById(int id) 
		{
			User user = await this._dbRepository.FindByIdAsync(id) ??
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return JsonConvert.SerializeObject(user);
		}

		public async Task<HttpStatusCode> UpdateUser(int id, UserDTO userDTO)
		{
			IEnumerable<User> allUsers = this._dbRepository.Query();

			bool userExists = false;
			foreach (var currUser in allUsers)
			{
				if (currUser.Id == userDTO.Id)
				{
					userExists = true;
					continue;
				}	

				if (currUser.UserName == userDTO.UserName)
					return HttpStatusCode.Forbidden;
			}

			if (!userExists)
				return HttpStatusCode.NotFound;

			User user = this._userMapper.Map<User>(userDTO);
			await this._dbRepository.EditAsync(id, user);
			return HttpStatusCode.OK;
		}

		public async Task<HttpStatusCode> DeleteUser(int id)
		{
			try // This skips having to query the database and check if the user doesn't exist
			{
				await this._dbRepository.DeleteAsync(id);
			}
			catch (ArgumentNullException)
			{
				return HttpStatusCode.NotFound;
			}
			
			return HttpStatusCode.OK;
		}
	}
}
