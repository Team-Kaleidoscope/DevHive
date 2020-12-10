using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Database;
using Microsoft.AspNetCore.Mvc;
using Models.Classes;
using Models.DTOs;
using Newtonsoft.Json;

namespace API.Service
{
	public class UserService
	{
		private readonly DbRepository<User> _dbRepository;

		public UserService(DevHiveContext context)
		{
			this._dbRepository = new DbRepository<User>(context);
		}
	
		[HttpPost]
		public async Task<HttpStatusCode> CreateUser(UserDTO userDTO)
		{
			//TODO: MAKE VALIDATIONS OF PROPER REQUEST
			//UserDTO newUser = JsonConvert.DeserializeObject<UserDTO>(userDTO);

			//await this._dbRepository.AddAsync(newUser);
			return HttpStatusCode.OK;
		}

		[HttpGet]
		public async Task<string> GetUserById(int id) 
		{
			User user = await this._dbRepository.FindByIdAsync(id);
			return JsonConvert.SerializeObject(user);
		}
	}
}
