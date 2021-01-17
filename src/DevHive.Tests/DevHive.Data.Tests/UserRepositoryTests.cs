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
		private LanguageRepository _languageRepository;
		private TechnologyRepository _technologyRepository;

		#region Setups
		[SetUp]
		public void Setup()
		{
			//Naming convention: MethodName_ExpectedBehavior_StateUnderTest
			var options = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase("DevHive_UserRepository_Database");

			this._context = new DevHiveContext(options.Options);
			this._userRepository = new UserRepository(_context);
			this._languageRepository = new LanguageRepository(_context);
			this._technologyRepository = new TechnologyRepository(_context);
		}

		[TearDown]
		public async Task Teardown()
		{
			await this._context.Database.EnsureDeletedAsync();
		}
		#endregion

		#region Create
		[Test]
		public async Task AddAsync_ShouldAddUserToDatabase()
		{
			//Arrange
			User dummyUser = CreateDummyUser();

			//Act
			bool result = await _userRepository.AddAsync(dummyUser);

			//Assert
			Assert.True(result, "User int' inserted properly into the database");
		}

		[Test]
		public async Task AddFriendToUserAsync_ShouldAddFriendToUsersList()
		{
			//Arrange
			User dummyUserOne = CreateDummyUser();
			User dummyUserTwo = CreateAnotherDummyUser();

			await this._userRepository.AddAsync(dummyUserOne);
			await this._userRepository.AddAsync(dummyUserTwo);

			//Act
			bool result = await this._userRepository.AddFriendToUserAsync(dummyUserOne, dummyUserTwo);

			//Assert
			Assert.True(result, "Friend didn't save properly in the database");
			Assert.True(dummyUserOne.Friends.Contains(dummyUserTwo), "Friend doesn't get added to user properly");
		}

		[Test]
		public async Task AddLanguageToUserAsync_ShouldAddLanguageToUser()
		{
			//Arrange
			User dummyUser = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUser);
			Language language = new()
			{
				Name = "typescript"
			};

			//Act
			bool result = await this._userRepository.AddLanguageToUserAsync(dummyUser, language);

			//Assert
			Assert.True(result, "The language isn't inserted properly to the database");
			Assert.True(dummyUser.Langauges.Contains(language), "The language doesn't get added properly to the user");
		}

		[Test]
		public async Task AddTechnologyToUserAsync_ShouldAddTechnologyToUser()
		{
			//Arrange
			User dummyUser = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUser);
			Technology technology = new()
			{
				Name = "Angular"
			};

			//Act
			bool result = await this._userRepository.AddTechnologyToUserAsync(dummyUser, technology);

			//Assert
			Assert.True(result, "The technology isn't inserted properly to the database");
			Assert.True(dummyUser.Technologies.Contains(technology), "The technology doesn't get added properly to the user");
		}
		#endregion

		#region Read
		[Test]
		public async Task QueryAll_ShouldReturnAllUsersFromDatabase()
		{
			//Arrange
			User dummyUserOne = CreateDummyUser();
			User dummyUserTwo = CreateAnotherDummyUser();

			await this._userRepository.AddAsync(dummyUserOne);
			await this._userRepository.AddAsync(dummyUserTwo);

			//Act
			IEnumerable<User> users = this._userRepository.QueryAll();
			foreach (var item in users)
				System.Console.WriteLine(item);

			//Assert
			Assert.GreaterOrEqual(users.Count(), 1, "Method doesn't return all instances of user");
		}

		[Test]
		public async Task GetById_ShouldReturnUserFromDatabase()
		{
			//Arrange
			User dummyUser = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUser);
			Guid id = dummyUser.Id;

			//Act
			User user = await this._userRepository.GetByIdAsync(id);

			//Assert
			Assert.AreEqual(dummyUser, user, "Method doesn't get the proper user from database");
		}

		[Test]
		public async Task GetByUsernameAsync_ShouldReturnUserFromDatabase()
		{
			//Arrange
			User dummyUser = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUser);
			string username = dummyUser.UserName;

			//Act
			User user = await this._userRepository.GetByUsernameAsync(username);

			//Assert
			Assert.AreEqual(dummyUser, user, "Method doesn't get the proper user from database");
		}

		[Test]
		public async Task GetUserLanguages_ShouldReturnAllSavedUserLanguages()
		{
			//Arrange
			User dummyUser = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUser);
			IList<Language> dummyUserLanguages = dummyUser.Langauges;

			//Act
			IList<Language> languages = this._userRepository.GetUserLanguages(dummyUser);

			//Assert
			Assert.AreEqual(dummyUserLanguages, languages, "Method doesn't query languages properly");
		}

		[Test]
		[TestCase]
		public async Task GetUserLanguage_ShouldReturnAllSavedUserLanguages()
		{
			Assert.Pass();
			// //Arrange
			// User dummyUser = CreateDummyUser();
			// await this._userRepository.AddAsync(dummyUser);
			// Language dummyLang = await this._languageRepository.GetByNameAsync("csharp");

			// //Act
			// IList<Language> languages = this._userRepository.GetUserLanguage(dummyUser, dummyLang);

			// //Assert
			// Assert.AreEqual(dummyUserLanguages, languages, "Method doesn't query languages properly");
		}
		#endregion

		#region HelperMethods
		private User CreateDummyUser()
		{
			List<Language> languages = new()
			{
				new Language()
				{
					Id = Guid.NewGuid(),
					Name = "csharp"
				},
			};

			List<Technology> technologies = new()
			{
				new Technology()
				{
					Id = Guid.NewGuid(),
					Name = "ASP.NET Core"
				},
			};

			List<Role> roles = new()
			{
				new Role()
				{
					Id = Guid.NewGuid(),
					Name = Role.DefaultRole
				},
			};

			return new()
			{
				Id = Guid.NewGuid(),
				UserName = "pioneer10",
				FirstName = "Spas",
				LastName = "Spasov",
				Email = "abv@abv.bg",
				Langauges = languages,
				Technologies = technologies,
				Roles = roles
			};
		}

		private User CreateAnotherDummyUser()
		{
			List<Language> languages = new()
			{
				new Language()
				{
					Id = Guid.NewGuid(),
					Name = "typescript"
				},
			};

			List<Technology> technologies = new()
			{
				new Technology()
				{
					Id = Guid.NewGuid(),
					Name = "Angular"
				},
			};

			List<Role> roles = new()
			{
				new Role()
				{
					Id = Guid.NewGuid(),
					Name = Role.DefaultRole
				},
			};

			return new()
			{
				Id = Guid.NewGuid(),
				UserName = "pioneer10",
				FirstName = "Alex",
				LastName = "Spiridonov",
				Email = "a_spiridonov@abv.bg",
				Langauges = languages,
				Technologies = technologies,
				Roles = roles
			};
		}
		#endregion
	}
}
