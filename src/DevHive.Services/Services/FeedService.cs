using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Interfaces.Repositories;
using DevHive.Data.Models;
using DevHive.Services.Interfaces;
using DevHive.Services.Models;
using DevHive.Services.Models.Post.Post;

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

		public async Task<ReadPageServiceModel> GetPage(GetPageServiceModel model)
		{
			User user = await this._userRepository.GetByIdAsync(model.UserId) ??
				throw new ArgumentException("User doesn't exist!");

			List<User> friendsList = user.Friends.ToList();
			// if(friendsList.Count == 0)
			// 	throw new ArgumentException("This user does not have any friends!");

			List<Post> posts = await this._feedRepository
				.GetFriendsPosts(friendsList, model.FirstRequestIssued, model.PageNumber, model.PageSize) ??
					throw new ArgumentException("No posts to query.");

			ReadPageServiceModel readPageServiceModel = new();
			foreach (Post post in posts)
				readPageServiceModel.Posts.Add(this._mapper.Map<ReadPostServiceModel>(post));

			return readPageServiceModel;
		}
	}
}
