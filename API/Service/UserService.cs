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
	
		public async Task<HttpStatusCode> CreateUser(UserDTO userDTO)
		{
			if (this._userDbRepository.DoesUsernameExist(userDTO.UserName))
				ThrowHttpRequestException(HttpStatusCode.BadRequest, "Username already exists!");

			User user = this._userMapper.Map<User>(userDTO);
			await this._userDbRepository.AddAsync(user);

			return HttpStatusCode.Created;
		}

		public async Task<User> GetUserById(int id) 
		{
			User user = await this._userDbRepository.FindByIdAsync(id);

			if (user == null)
				ThrowHttpRequestException(HttpStatusCode.NotFound);

			return user;
		}

		public async Task<HttpStatusCode> UpdateUser(int id, UserDTO userDTO)
		{
			if (!this._userDbRepository.DoesUserExist(id))
				ThrowHttpRequestException(HttpStatusCode.NotFound);

			if (this._userDbRepository.DoesUsernameExist(userDTO.UserName))
				ThrowHttpRequestException(HttpStatusCode.Forbidden);

			User user = this._userMapper.Map<User>(userDTO);
			await this._userDbRepository.EditAsync(id, user);

			return HttpStatusCode.OK;
		}

		public async Task<HttpStatusCode> DeleteUser(int id)
		{
			if (!this._userDbRepository.DoesUserExist(id))
				return HttpStatusCode.Forbidden;

			await this._userDbRepository.DeleteAsync(id);
			
			return HttpStatusCode.OK;
		}

		private void ThrowHttpRequestException(HttpStatusCode statusCode, string errorMessage = "")
		{
			HttpResponseMessage message = new()
			{
				StatusCode = statusCode,
				Content = new StringContent(errorMessage)
			};

			throw new HttpResponseException(message);
		}
	}
}
