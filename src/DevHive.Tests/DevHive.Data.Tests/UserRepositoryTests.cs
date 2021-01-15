using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
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
		private User _dummyUser;

		[SetUp]
		public void Setup()
		{
			//Naming convention: MethodName_ExpectedBehavior_StateUnderTest
			var options = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase("DevHive_UserRepository_Database");

			this._context = new DevHiveContext(options.Options);
			this._userRepository = new UserRepository(_context);

			this._dummyUser = new Mock<User>().Object;

			foreach (var item in _dummyUser.Langauges)
				System.Console.WriteLine(item);

			foreach (var item in _dummyUser.Technologies)
				System.Console.WriteLine(item);
		}

		[Test]
		public void AddAsync_ShouldAddUserToDatabase()
		{

		}
	}
}
