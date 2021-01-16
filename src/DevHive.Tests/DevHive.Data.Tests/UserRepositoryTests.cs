using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DevHive.Data.Tests
{
	[TestFixture]
	public class UserRepositoryTests
	{
		private DevHiveContext _context;
		private UserRepository _userRepository;

		private List<Language> _dummyLanguageList;
		private List<Technology> _dummyTechnologyList;
		private List<Role> _dummyRoleList;
		private User _dummyUser;
		private User _dummyUserTwo;

		[SetUp]
		public void Setup()
		{
			//Naming convention: MethodName_ExpectedBehavior_StateUnderTest
			var options = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase("DevHive_UserRepository_Database");

			this._context = new DevHiveContext(options.Options);
			this._userRepository = new UserRepository(_context);

			this._dummyLanguageList = new()
			{
				new Language()
				{
					Id = Guid.NewGuid(),
					Name = "csharp"
				},
			};

			this._dummyTechnologyList = new()
			{
				new Technology()
				{
					Id = Guid.NewGuid(),
					Name = "ASP.NET Core"
				},
			};

			this._dummyRoleList = new()
			{
				new Role()
				{
					Id = Guid.NewGuid(),
					Name = Role.DefaultRole
				},
			};

			this._dummyUser = new()
			{
				Id = Guid.NewGuid(),
				UserName = "pioneer10",
				FirstName = "Spas",
				LastName = "Spasov",
				Email = "abv@abv.bg",
				Langauges = this._dummyLanguageList,
				Technologies = this._dummyTechnologyList,
				Roles = this._dummyRoleList
			};

			this._dummyUserTwo = new()
			{
				Id = Guid.NewGuid(),
				UserName = "pioneer10",
				FirstName = "Alex",
				LastName = "Spiridonov",
				Email = "a_spiridonov@abv.bg",
				Langauges = this._dummyLanguageList,
				Technologies = this._dummyTechnologyList,
				Roles = this._dummyRoleList
			};
		}

		[TearDown]
		public void Teardown()
		{
			this._context.Database.EnsureDeleted();
		}

		#region Create
		[Test]
		public async Task AddAsync_ShouldAddUserToDatabase()
		{
			//Arrange

			//Act
			bool result = await _userRepository.AddAsync(this._dummyUser);

			//Assert
			Assert.True(result, "User does not insert into database properly");
		}

		[Test]
		public async Task AddFriendToUserAsync_ShouldAddFriendToUsersList()
		{
			//Arrange
			await this._userRepository.AddAsync(this._dummyUser);
			await this._userRepository.AddAsync(this._dummyUserTwo);

			//Act
			bool result = await this._userRepository.AddFriendToUserAsync(this._dummyUser, _dummyUserTwo);

			//Assert
			Assert.True(result, "Friend didn't save properly in the database");
			Assert.True(this._dummyUser.Friends.Contains(_dummyUserTwo), "Friend doesn't get added to user properly");
		}

		[Test]
		public async Task AddLanguageToUserAsync_ShouldAddLanguageToUser()
		{
			//Arrange
			bool added = await this._userRepository.AddAsync(this._dummyUser);
			Assert.True(added, "User not inserted properly!");
			Language language = new()
			{
				Id = Guid.NewGuid(),
				Name = "typescript"
			};

			//Act
			bool result = await this._userRepository.AddLanguageToUserAsync(this._dummyUser, language);

			//Assert
			Assert.True(result, "The language isn't inserted properly to the database");
			Assert.True(this._dummyUser.Langauges.Contains(language), "The language doesn't get added properly to the user");
		}

		[Test]
		public async Task AddTechnologyToUserAsync_ShouldAddTechnologyToUser()
		{
			//Arrange
			await this._userRepository.AddAsync(this._dummyUser);
			Technology technology = new()
			{
				Id = Guid.NewGuid(),
				Name = "Angular"
			};

			//Act
			bool result = await this._userRepository.AddTechnologyToUserAsync(this._dummyUser, technology);

			//Assert
			Assert.True(result, "The technology isn't inserted properly to the database");
			Assert.True(this._dummyUser.Technologies.Contains(technology), "The technology doesn't get added properly to the user");
		}
		#endregion

		#region Read
		[Test]
		public async Task QueryAll_ShouldReturnAllUsersFromDatabase()
		{
			//Arrange
			await this._userRepository.AddAsync(this._dummyUser);
			await this._userRepository.AddAsync(this._dummyUserTwo);

			//Act
			IEnumerable<User> users = this._userRepository.QueryAll();

			//Assert
			Assert.GreaterOrEqual(users.Count(), 1, "Query all does not return all instances of user");
		}
		#endregion
	}
}
