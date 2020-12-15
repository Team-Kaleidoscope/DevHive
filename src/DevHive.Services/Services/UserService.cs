using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Options;
using DevHive.Services.Models.Identity.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevHive.Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

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

		public async Task<IActionResult> LoginUser(LoginServiceModel loginModel)
		{
			if (!await this._userRepository.IsUsernameValid(loginModel.UserName))
				return new BadRequestObjectResult("Invalid username!");

			User user = await this._userRepository
				.GetByUsername(loginModel.UserName);

			if (user.PasswordHash != GeneratePasswordHash(loginModel.Password))
				return new BadRequestObjectResult("Incorrect password!");

			return new OkObjectResult(new 
			{
				Token = WriteJWTSecurityToken(user.Role) 
			});
		}

		public async Task<IActionResult> RegisterUser(RegisterServiceModel registerModel)
		{
			if (await this._userRepository.DoesUsernameExist(registerModel.UserName))
				return new BadRequestObjectResult("Username already exists!");

			if (await this._userRepository.DoesEmailExist(registerModel.Email))
				return new BadRequestObjectResult("Email already exists!");

			User user = this._userMapper.Map<User>(registerModel);
			user.Role = Role.DefaultRole;
			user.PasswordHash = GeneratePasswordHash(registerModel.Password);

			await this._userRepository.AddAsync(user);

			return new CreatedResult("CreateUser", user);
		}

		public async Task<IActionResult> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id);

			if (user == null)
				return new NotFoundObjectResult("User does not exist!");

			return new OkObjectResult(user);
		}

		public async Task<IActionResult> UpdateUser(UpdateUserServiceModel updateModel)
		{
			if (!this._userRepository.DoesUserExist(updateModel.Id))
				return new NotFoundObjectResult("User does not exist!");

			if (!this._userRepository.DoesUserHaveThisUsername(updateModel.Id, updateModel.UserName)
					&& await this._userRepository.IsUsernameValid(updateModel.UserName))
				return new BadRequestObjectResult("Username already exists!");

			User user = this._userMapper.Map<User>(updateModel);
			await this._userRepository.EditAsync(user);

			return new AcceptedResult("UpdateUser", user);
		}

		public async Task<IActionResult> DeleteUser(Guid id)
		{
			if (!this._userRepository.DoesUserExist(id))
				return new NotFoundObjectResult("User does not exist!");

			User user = await this._userRepository.GetByIdAsync(id);
			await this._userRepository.DeleteAsync(user);

			return new OkResult();
		}

		private string GeneratePasswordHash(string password)
		{
			return string.Join(string.Empty, SHA512.HashData(Encoding.ASCII.GetBytes(password)));
		}

		private string WriteJWTSecurityToken(string role)
		{
			//TODO: Try generating the key
			byte[] signingKey = Convert.FromBase64String(_jwtOptions.Secret);
			
			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Role, role)
				}),
				Expires = DateTime.Today.AddDays(7),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(signingKey),
					SecurityAlgorithms.HmacSha512Signature)
			};

			JwtSecurityTokenHandler tokenHandler = new();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
