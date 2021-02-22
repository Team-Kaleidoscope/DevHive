using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
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
			DbContextOptionsBuilder<DevHiveContext> options = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase("DevHive_UserRepository_Database");
			this._context = new DevHiveContext(options.Options);

			Guid userId = Guid.NewGuid();
			Mock<IUserStore<User>> userStore = new();
			userStore.Setup(x => x.FindByIdAsync(userId.ToString(), CancellationToken.None))
				.ReturnsAsync(new User()
				{
					Id = userId,
					UserName = "test",
				});
			Mock<UserManager<User>> userManagerMock = new(userStore.Object, null, null, null, null, null, null, null, null);

			Guid roleId = Guid.NewGuid();
			Mock<IRoleStore<Role>> roleStore = new();
			roleStore.Setup(x => x.FindByIdAsync(roleId.ToString(), CancellationToken.None))
				.ReturnsAsync(new Role()
				{
					Id = roleId,
					Name = "test",
				});
			Mock<RoleManager<Role>> roleManagerMock = new(roleStore.Object, null, null, null, null);
			this._userRepository = new(this._context, userManagerMock.Object, roleManagerMock.Object);
		}

		[TearDown]
		public async Task Teardown()
		{
			_ = await this._context.Database.EnsureDeletedAsync();
		}
		#endregion

		#region EditAsync
		[Test]
		public async Task EditAsync_ReturnsTrue_WhenUserIsUpdatedSuccessfully()
		{
			User oldUser = CreateDummyUser();
			_ = this._context.Users.Add(oldUser);
			_ = await this._context.SaveChangesAsync();

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
			_ = await this._userRepository.AddAsync(dummyUserOne);

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
			_ = await this._userRepository.AddAsync(dummyUser);
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
			User dummyUser = CreateDummyUser();
			_ = this._context.Users.Add(dummyUser);
			_ = await this._context.SaveChangesAsync();

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
			User dummyUser = CreateDummyUser();
			_ = this._context.Users.Add(dummyUser);
			_ = await this._context.SaveChangesAsync();

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
			User dummyUser = CreateDummyUser();
			_ = this._context.Users.Add(dummyUser);
			_ = await this._context.SaveChangesAsync();

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

		#region DoesUserHaveThisUsernameAsync
		[Test]
		public async Task DoesUserHaveThisUsername_ReturnsTrue_WhenUserHasTheGivenUsername()
		{
			User dummyUser = CreateDummyUser();
			_ = this._context.Users.Add(dummyUser);
			_ = await this._context.SaveChangesAsync();

			bool result = await this._userRepository.DoesUserHaveThisUsernameAsync(dummyUser.Id, dummyUser.UserName);

			Assert.IsTrue(result, "DoesUserHaveThisUsernameAsync does not return true when the user has the given name");
		}

		[Test]
		public async Task DoesUserHaveThisUsername_ReturnsFalse_WhenUserDoesntHaveTheGivenUsername()
		{
			string username = "Fake username";
			User dummyUser = CreateDummyUser();
			_ = this._context.Users.Add(dummyUser);
			_ = await this._context.SaveChangesAsync();

			bool result = await this._userRepository.DoesUserHaveThisUsernameAsync(dummyUser.Id, username);

			Assert.IsFalse(result, "DoesUserNameExistAsync does not return false when user doesnt have the given name");
		}
		#endregion

		#region HelperMethods
		private static User CreateDummyUser()
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
		#endregion
	}
}
