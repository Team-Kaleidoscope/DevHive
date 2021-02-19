using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.Message;

namespace DevHive.Services.Services
{
	public class MessageService : IMessageService
	{
		private readonly MessageRepository _messageRepository;
		private readonly UserRepository _userRepository;

		public MessageService(MessageRepository messageRepository, UserRepository userRepository)
		{
			this._messageRepository = messageRepository;
			this._userRepository = userRepository;
		}

		public Task<Guid> CreateMessage(CreateMessageServiceModel createMessageServiceModel)
		{
			throw new NotImplementedException();
		}

		public Task<ReadMessageServiceModel> GetMessageById(Guid id)
		{
			throw new NotImplementedException();
		}

		#region Validations
		/// <summary>
		/// Checks whether the user Id in the token and the given user Id match
		/// </summary>
		public async Task<bool> ValidateJwtForCreating(Guid userId, string rawTokenData)
		{
			User user = await this.GetUserForValidation(rawTokenData);

			return user.Id == userId;
		}

		/// <summary>
		/// Checks whether the comment, gotten with the commentId,
		/// is made by the user in the token
		/// or if the user in the token is an admin
		/// </summary>
		public async Task<bool> ValidateJwtForMessage(Guid messageId, string rawTokenData)
		{
			Message message = await this._messageRepository.GetByIdAsync(messageId) ??
				throw new ArgumentException("Message does not exist!");
			User user = await this.GetUserForValidation(rawTokenData);

			//If user made the comment
			if (message.Creator.Id == user.Id)
				return true;
			//If user is admin
			else if (user.Roles.Any(x => x.Name == Role.AdminRole))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Returns the user, via their Id in the token
		/// </summary>
		private async Task<User> GetUserForValidation(string rawTokenData)
		{
			JwtSecurityToken jwt = new JwtSecurityTokenHandler().ReadJwtToken(rawTokenData.Remove(0, 7));

			Guid jwtUserId = Guid.Parse(this.GetClaimTypeValues("ID", jwt.Claims).First());

			User user = await this._userRepository.GetByIdAsync(jwtUserId) ??
				throw new ArgumentException("User does not exist!");

			return user;
		}

		/// <summary>
		/// Returns all values from a given claim type
		/// </summary>
		private List<string> GetClaimTypeValues(string type, IEnumerable<Claim> claims)
		{
			List<string> toReturn = new();

			foreach (var claim in claims)
				if (claim.Type == type)
					toReturn.Add(claim.Value);

			return toReturn;
		}
		#endregion
	}
}
