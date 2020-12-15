using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using DevHive.Data.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace DevHive.Services.Services
{
	public class UserService
	{
		private readonly UserRepository _userRepository;
		private readonly IMapper _userMapper;
		private readonly JWTOptions _jwtOptions;

		public UserService(DevHiveContext context, IMapper mapper, JWTOptions jwtOptions)
		{
			this._userRepository = new UserRepository(context);
			this._userMapper = mapper;
			this._jwtOptions = jwtOptions;
		}

		public async Task<IActionResult> LoginUser(LoginDTO loginDTO)
		{
			User user = this._userRepository.FindByUsername(loginDTO.UserName);

			if (user == null)
				return new NotFoundObjectResult("User does not exist!");

			byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);  

			if (user.PasswordHash != GeneratePasswordHash(loginDTO.Password))
				return new BadRequestObjectResult("Incorrect password!");

			// Create Jwt Token configuration
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Role, user.Role) // Authorize user by role
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			return new OkObjectResult(new { Token = tokenString });
		}

		public async Task<IActionResult> RegisterUser(RegisterDTO registerDTO)
		{

			if (this._userRepository.DoesUsernameExist(registerDTO.UserName))
				return new BadRequestObjectResult("Username already exists!");

			User user = this._userMapper.Map<User>(registerDTO);

			user.Role = UserRoles.User;
			user.PasswordHash = GeneratePasswordHash(registerDTO.Password);

			await this._userRepository.AddAsync(user);

			return new CreatedResult("CreateUser", user);
		}

		private string GeneratePasswordHash(string password)
		{
			//TODO: Implement
			return password;
		}

		public async Task<IActionResult> GetUserById(Guid id) 
		{
			User user = await this._userRepository.FindByIdAsync(id);

			if (user == null)
				return new NotFoundObjectResult("User does not exist!");

			return new OkObjectResult(user);
		}

		public async Task<IActionResult> UpdateUser(Guid id, UserDTO userDTO)
		{
			if (!this._userRepository.DoesUserExist(id))
				return new NotFoundObjectResult("User does not exist!");

			if (!this._userRepository.HasThisUsername(id, userDTO.UserName)
					&& this._userRepository.DoesUsernameExist(userDTO.UserName))
				return new BadRequestObjectResult("Username already exists!");

			User user = this._userMapper.Map<User>(userDTO);
			await this._userRepository.EditAsync(id, user);

			return new AcceptedResult("UpdateUser", user);
		}

		public async Task<IActionResult> DeleteUser(Guid id)
		{
			if (!this._userRepository.DoesUserExist(id))
				return new NotFoundObjectResult("User does not exist!");

			await this._userDbRepository.DeleteAsync(id);
			
			return new OkResult();
		}
	}
}
