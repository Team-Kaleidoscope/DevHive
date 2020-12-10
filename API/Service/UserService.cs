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
			//TODO: MAKE VALIDATIONS OF PROPER REQUEST

			User user = this._userMapper.Map<User>(userDTO);
			await this._dbRepository.AddAsync(user);

			return HttpStatusCode.OK;
		}

		public async Task<string> GetUserById(int id) 
		{
			User user = await this._dbRepository.FindByIdAsync(id);
			return JsonConvert.SerializeObject(user);
		}

		public async Task<HttpStatusCode> UpdateUser(int id, UserDTO userDTO)
		{
			User user = this._userMapper.Map<User>(userDTO);
			await this._dbRepository.EditAsync(id, user);

			return HttpStatusCode.OK;
		}

		public async Task<HttpStatusCode> DeleteUser(int id)
		{
			await this._dbRepository.DeleteAsync(id);

			return HttpStatusCode.OK;
		}
	}
}
