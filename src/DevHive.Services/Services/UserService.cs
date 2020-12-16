using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Options;
using DevHive.Services.Models.Identity.User;
using System.Threading.Tasks;
using DevHive.Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using DevHive.Common.Models.Identity;

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

		public async Task<TokenModel> LoginUser(LoginServiceModel loginModel)
		{
			if (!await this._userRepository.IsUsernameValid(loginModel.UserName))
				throw new ArgumentException("Invalid username!");

			User user = await this._userRepository.GetByUsername(loginModel.UserName);

			if (user.PasswordHash != GeneratePasswordHash(loginModel.Password))
				throw new ArgumentException("Incorrect password!");

			return new TokenModel(WriteJWTSecurityToken(user.Role));
		}

		public async Task<TokenModel> RegisterUser(RegisterServiceModel registerModel)
		{
			if (await this._userRepository.DoesUsernameExist(registerModel.UserName))
				throw new ArgumentException("Username already exists!");

			if (await this._userRepository.DoesEmailExist(registerModel.Email))
				throw new ArgumentException("Email already exists!");

			User user = this._userMapper.Map<User>(registerModel);
			user.Role = Role.DefaultRole;
			user.PasswordHash = GeneratePasswordHash(registerModel.Password);

			await this._userRepository.AddAsync(user);

			return new TokenModel(WriteJWTSecurityToken(user.Role));
		}

		public async Task<UserServiceModel> GetUserById(Guid id)
		{
			User user = await this._userRepository.GetByIdAsync(id)
				?? throw new ArgumentException("User does not exist!");

			return this._userMapper.Map<UserServiceModel>(user);
		}

		public async Task<UserServiceModel> UpdateUser(UpdateUserServiceModel updateModel)
		{
			if (!this._userRepository.DoesUserExist(updateModel.Id))
				throw new ArgumentException("User does not exist!");

			if (!this._userRepository.DoesUserHaveThisUsername(updateModel.Id, updateModel.UserName)
					&& await this._userRepository.IsUsernameValid(updateModel.UserName))
				throw new ArgumentException("Username already exists!");

			User user = this._userMapper.Map<User>(updateModel);
			bool result = await this._userRepository.EditAsync(user);

			if (!result)
				throw new InvalidOperationException("Unable to edit user!");

			return this._userMapper.Map<UserServiceModel>(user);;
		}

		public async Task DeleteUser(Guid id)
		{
			if (!this._userRepository.DoesUserExist(id))
				throw new ArgumentException("User does not exist!");

			User user = await this._userRepository.GetByIdAsync(id);
			bool result = await this._userRepository.DeleteAsync(user);

			if (!result)
				throw new InvalidOperationException("Unable to delete user!");
		}

		private string GeneratePasswordHash(string password)
		{
			return string.Join(string.Empty, SHA512.HashData(Encoding.ASCII.GetBytes(password)));
		}

		private string WriteJWTSecurityToken(string role)
		{
			//TODO: Try generating the key
			byte[] signingKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, role)
			};

			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(claims),
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
