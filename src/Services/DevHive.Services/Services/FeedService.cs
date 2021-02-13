using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models;
using DevHive.Services.Models.Post;

namespace DevHive.Services.Services
{
	public class FeedService : IFeedService
	{
		private readonly IMapper _mapper;
		private readonly IFeedRepository _feedRepository;
		private readonly IUserRepository _userRepository;

		public FeedService(IFeedRepository feedRepository, IUserRepository userRepository, IMapper mapper)
		{
			this._feedRepository = feedRepository;
			this._userRepository = userRepository;
			this._mapper = mapper;
		}

		/// <summary>
		/// This method is used in the feed page.
		/// See the FeedRepository "GetFriendsPosts" menthod for more information on how it works.
        /// </summary>
		public async Task<ReadPageServiceModel> GetPage(GetPageServiceModel model)
		{
			User user = null;

			if (model.UserId != Guid.Empty)
				user = await this._userRepository.GetByIdAsync(model.UserId);
			else if (!string.IsNullOrEmpty(model.Username))
				user = await this._userRepository.GetByUsernameAsync(model.Username);
			else
				throw new ArgumentException("Invalid given data!");

			if (user == null)
				throw new ArgumentException("User doesn't exist!");

			if (user.Friends.Count == 0)
				throw new ArgumentException("User has no friends to get feed from!");

			List<Post> posts = await this._feedRepository
				.GetFriendsPosts(user.Friends.ToList(), model.FirstRequestIssued, model.PageNumber, model.PageSize);

			ReadPageServiceModel readPageServiceModel = new();
			foreach (Post post in posts)
				readPageServiceModel.Posts.Add(this._mapper.Map<ReadPostServiceModel>(post));

			return readPageServiceModel;
		}

		/// <summary>
		/// This method is used in the profile pages.
		/// See the FeedRepository "GetUsersPosts" menthod for more information on how it works.
        /// </summary>
		public async Task<ReadPageServiceModel> GetUserPage(GetPageServiceModel model)
		{
			User user = null;

			if (!string.IsNullOrEmpty(model.Username))
				user = await this._userRepository.GetByUsernameAsync(model.Username);
			else
				throw new ArgumentException("Invalid given data!");

			if (user == null)
				throw new ArgumentException("User doesn't exist!");

			List<Post> posts = await this._feedRepository
				.GetUsersPosts(user, model.FirstRequestIssued, model.PageNumber, model.PageSize);

			ReadPageServiceModel readPageServiceModel = new();
			foreach (Post post in posts)
				readPageServiceModel.Posts.Add(this._mapper.Map<ReadPostServiceModel>(post));

			return readPageServiceModel;
		}
	}
}
