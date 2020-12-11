using System.Net;
using System.Threading.Tasks;
using API.Database;
using AutoMapper;
using Models.Classes;
using Models.DTOs;
using Newtonsoft.Json;
using System.Web.Http;

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
			if(this._userDbRepository.DoesUsernameExist(userDTO.UserName))
				return HttpStatusCode.Forbidden;

			User user = this._userMapper.Map<User>(userDTO);
			await this._userDbRepository.AddAsync(user);

			return HttpStatusCode.OK;
		}

		public async Task<string> GetUserById(int id) 
		{
			User user = await this._userDbRepository.FindByIdAsync(id) ??
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return JsonConvert.SerializeObject(user);
		}

		public async Task<HttpStatusCode> UpdateUser(int id, UserDTO userDTO)
		{
			if (!this._userDbRepository.DoesUserExist(id))
				return HttpStatusCode.NotFound;

			if (this._userDbRepository.DoesUsernameExist(userDTO.UserName))
				return HttpStatusCode.Forbidden;

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
	}
}
