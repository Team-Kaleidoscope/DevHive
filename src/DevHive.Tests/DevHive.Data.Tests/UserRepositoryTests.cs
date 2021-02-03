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

		#region Setups
		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase("DevHive_UserRepository_Database");

			this._context = new DevHiveContext(options.Options);
			this._userRepository = new UserRepository(_context);
		}

		[TearDown]
		public async Task Teardown()
		{
			await this._context.Database.EnsureDeletedAsync();
		}
		#endregion

		#region QueryAll
		[Test]
		public async Task QueryAll_ShouldReturnAllUsersFromDatabase_WhenTheyExist()
		{
			//Arrange
			User dummyUserOne = CreateDummyUser();
			User dummyUserTwo = CreateAnotherDummyUser();

			await this._userRepository.AddAsync(dummyUserOne);
			await this._userRepository.AddAsync(dummyUserTwo);

			//Act
			IEnumerable<User> users = this._userRepository.QueryAll();

			//Assert
			Assert.AreEqual(2, users.Count(), "Method doesn't return all instances of user");
		}

		[Test]
		public void QueryAll_ReturnsNull_WhenNoUsersExist()
		{
			IEnumerable<User> users = this._userRepository.QueryAll();

			Assert.AreEqual(0, users.Count(), "Method returns Users when there are non");
		}
		#endregion

		#region EditAsync
		[Test]
		public async Task EditAsync_ReturnsTrue_WhenUserIsUpdatedSuccessfully()
		{
			User oldUser = this.CreateDummyUser();
			this._context.Users.Add(oldUser);
			await this._context.SaveChangesAsync();

			oldUser.UserName = "SuperSecretUserName";
			bool result = await this._userRepository.EditAsync(oldUser.Id, oldUser);

			Assert.IsTrue(result, "EditAsync does not return true when User is updated successfully");
		}
		#endregion

		#region GetByIdAsync
		[Test]
		public async Task GetByIdAsync_ReturnsTheUse_WhenItExists()
		{
			User dummyUserOne = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUserOne);

			User resultUser = await this._userRepository.GetByIdAsync(dummyUserOne.Id);

			Assert.AreEqual(dummyUserOne.UserName, resultUser.UserName);
		}

		[Test]
		public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
		{
			Guid id = Guid.NewGuid();

			User resultUser = await this._userRepository.GetByIdAsync(id);

			Assert.IsNull(resultUser);
		}
		#endregion

		#region GetByUsernameAsync
		[Test]
		public async Task GetByUsernameAsync_ReturnsUserFromDatabase_WhenItExists()
		{
			//Arrange
			User dummyUser = CreateDummyUser();
			await this._userRepository.AddAsync(dummyUser);
			string username = dummyUser.UserName;

			//Act
			User user = await this._userRepository.GetByUsernameAsync(username);

			//Assert
			Assert.AreEqual(dummyUser.Id, user.Id, "Method doesn't get the proper user from database");
		}

		[Test]
		public async Task GetByUsernameAsync_ReturnsNull_WhenUserDoesNotExist()
		{
			//Act
			User user = await this._userRepository.GetByUsernameAsync(null);

			//Assert
			Assert.IsNull(user, "Method returns user when it does not exist");
		}
		#endregion

		#region DoesUserExistAsync
		[Test]
		public async Task DoesUserExistAsync_ReturnsTrue_WhenUserExists()
		{
			User dummyUser = this.CreateDummyUser();
			this._context.Users.Add(dummyUser);
			await this._context.SaveChangesAsync();

			bool result = await this._userRepository.DoesUserExistAsync(dummyUser.Id);

			Assert.IsTrue(result, "DoesUserExistAsync does not return true when user exists");
		}

		[Test]
		public async Task DoesUserExistAsync_ReturnsFalse_WhenUserDoesNotExist()
		{
			Guid id = Guid.NewGuid();

			bool result = await this._userRepository.DoesUserExistAsync(id);

			Assert.IsFalse(result, "DoesUserExistAsync does not return false when user does not exist");
		}
		#endregion

		#region DoesUserNameExistAsync
		[Test]
		public async Task DoesUsernameExistAsync_ReturnsTrue_WhenUserWithTheNameExists()
		{
			User dummyUser = this.CreateDummyUser();
			this._context.Users.Add(dummyUser);
			await this._context.SaveChangesAsync();

			bool result = await this._userRepository.DoesUsernameExistAsync(dummyUser.UserName);

			Assert.IsTrue(result, "DoesUserNameExistAsync does not return true when username exists");
		}

		[Test]
		public async Task DoesUsernameExistAsync_ReturnsFalse_WhenUserWithTheNameDoesNotExist()
		{
			string userName = "Fake name";

			bool result = await this._userRepository.DoesUsernameExistAsync(userName);

			Assert.IsFalse(result, "DoesUserNameExistAsync does not return false when username does not exist");
		}
		#endregion

		#region DoesEmailExistAsync
		[Test]
		public async Task DoesEmailExistAsync_ReturnsTrue_WhenUserWithTheEmailExists()
		{
			User dummyUser = this.CreateDummyUser();
			this._context.Users.Add(dummyUser);
			await this._context.SaveChangesAsync();

			bool result = await this._userRepository.DoesEmailExistAsync(dummyUser.Email);

			Assert.IsTrue(result, "DoesUserNameExistAsync does not return true when email exists");
		}

		[Test]
		public async Task DoesEmailExistAsync_ReturnsFalse_WhenUserWithTheEmailDoesNotExist()
		{
			string email = "Fake email";

			bool result = await this._userRepository.DoesUsernameExistAsync(email);

			Assert.IsFalse(result, "DoesUserNameExistAsync does not return false when email does not exist");
		}
		#endregion

		#region DoesUserHaveThisFriendAsync
		//[Test]
		//public async Task DoesUserHaveThisFriendAsync_ReturnsTrue_WhenUserHasTheGivenFriend()
		//{
		//	User dummyUser = this.CreateDummyUser();
		//	User anotherDummyUser = this.CreateAnotherDummyUser();
		//	HashSet<User> friends = new HashSet<User>
		//	{
		//		anotherDummyUser
		//	};
		//	dummyUser.Friends = friends;

		//	this._context.Users.Add(dummyUser);
		//	this._context.Users.Add(anotherDummyUser);
		//	await this._context.SaveChangesAsync();

		//	bool result = await this._userRepository.DoesUserHaveThisFriendAsync(dummyUser.Id, anotherDummyUser.Id);

		//	Assert.IsTrue(result, "DoesUserHaveThisFriendAsync does not return true when user has the given friend");
		//}

		[Test]
		public async Task DoesUserHaveThisFriendAsync_ReturnsFalse_WhenUserDoesNotHaveTheGivenFriend()
		{
			User dummyUser = this.CreateDummyUser();
			User anotherDummyUser = this.CreateAnotherDummyUser();

			this._context.Users.Add(dummyUser);
			this._context.Users.Add(anotherDummyUser);
			await this._context.SaveChangesAsync();

			bool result = await this._userRepository.DoesUserHaveThisFriendAsync(dummyUser.Id, anotherDummyUser.Id);

			Assert.IsFalse(result, "DoesUserHaveThisFriendAsync does not return false when user des not have the given friend");
		}
		#endregion

		#region DoesUserHaveThisUsername
		[Test]
		public async Task DoesUserHaveThisUsername_ReturnsTrue_WhenUserHasTheGivenUsername()
		{
			User dummyUser = this.CreateDummyUser();
			this._context.Users.Add(dummyUser);
			await this._context.SaveChangesAsync();

			bool result = this._userRepository.DoesUserHaveThisUsername(dummyUser.Id, dummyUser.UserName);

			Assert.IsTrue(result, "DoesUserHaveThisUsername does not return true when the user has the given name");
		}

		[Test]
		public async Task DoesUserHaveThisUsername_ReturnsFalse_WhenUserDoesntHaveTheGivenUsername()
		{
			string username = "Fake username";
			User dummyUser = this.CreateDummyUser();
			this._context.Users.Add(dummyUser);
			await this._context.SaveChangesAsync();

			bool result = this._userRepository.DoesUserHaveThisUsername(dummyUser.Id, username);

			Assert.IsFalse(result, "DoesUserNameExistAsync does not return false when user doesnt have the given name");
		}
		#endregion

		#region HelperMethods
		private User CreateDummyUser()
		{
			HashSet<Language> languages = new()
			{
				new Language()
				{
					Id = Guid.NewGuid(),
					Name = "csharp"
				},
			};

			HashSet<Technology> technologies = new()
			{
				new Technology()
				{
					Id = Guid.NewGuid(),
					Name = "ASP.NET Core"
				},
			};

			HashSet<Role> roles = new()
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
				UserName = "dummyUser",
				FirstName = "Spas",
				LastName = "Spasov",
				Email = "abv@abv.bg",
				Languages = languages,
				Technologies = technologies,
				Roles = roles
			};
		}

		private User CreateAnotherDummyUser()
		{
			HashSet<Language> languages = new()
			{
				new Language()
				{
					Id = Guid.NewGuid(),
					Name = "typescript"
				},
			};

			HashSet<Technology> technologies = new()
			{
				new Technology()
				{
					Id = Guid.NewGuid(),
					Name = "Angular"
				},
			};

			HashSet<Role> roles = new()
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
				UserName = "anotherDummyUser",
				FirstName = "Alex",
				LastName = "Spiridonov",
				Email = "a_spiridonov@abv.bg",
				Languages = languages,
				Technologies = technologies,
				Roles = roles
			};
		}
		#endregion
	}
}
