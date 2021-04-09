using System;
using System.IO;
using System.Threading.Tasks;
using DevHive.Data;
using DevHive.Data.Interfaces;
using DevHive.Data.Models;
using DevHive.Data.Repositories;
using DevHive.Services.Interfaces;
using DevHive.Services.Models.ProfilePicture;
using DevHive.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DevHive.Services.Tests
{
	[TestFixture]
	public class ProfilePictureServiceTests
	{
		private DevHiveContext _context;
		private Mock<IUserRepository> _userRepositoryMock;
		private Mock<ProfilePictureRepository> _profilePictureRepository;
		private Mock<ICloudService> _cloudService;
		private ProfilePictureService _profilePictureService;

		[SetUp]
		public void Setup()
		{
			DbContextOptionsBuilder<DevHiveContext> options = new DbContextOptionsBuilder<DevHiveContext>()
				.UseInMemoryDatabase("DevHive_UserRepository_Database");
			this._context = new DevHiveContext(options.Options);
			this._userRepositoryMock = new Mock<IUserRepository>();
			this._profilePictureRepository = new Mock<ProfilePictureRepository>(this._context);
			this._cloudService = new Mock<ICloudService>();

			this._profilePictureService = new ProfilePictureService(
				this._userRepositoryMock.Object,
				this._profilePictureRepository.Object,
				this._cloudService.Object
			);
		}

		// [Test]
		// public async Task InsertProfilePicture_ShouldInsertProfilePicToDatabaseAndUploadToCloud()
		// {
		// 	//Arrange
		// 	Guid userId = Guid.NewGuid();
		// 	Mock<IFormFile> fileMock = new();
        //
		// 	//File mocking setup
		// 	var content = "Hello World from a Fake File";
		// 	var fileName = "test.jpg";
		// 	var ms = new MemoryStream();
		// 	var writer = new StreamWriter(ms);
		// 	writer.Write(content);
		// 	writer.Flush();
		// 	ms.Position = 0;
		// 	fileMock.Setup(p => p.FileName).Returns(fileName);
		// 	fileMock.Setup(p => p.Length).Returns(ms.Length);
		// 	fileMock.Setup(p => p.OpenReadStream()).Returns(ms);
        //
		// 	//User Setup
		// 	this._userRepositoryMock
		// 		.Setup(p => p.GetByIdAsync(userId))
		// 		.ReturnsAsync(new User()
		// 		{
		// 			Id = userId
		// 		});
        //
		// 	ProfilePictureServiceModel profilePictureServiceModel = new()
		// 	{
		// 		UserId = userId,
		// 		ProfilePictureFormFile = fileMock.Object
		// 	};
        //
		// 	//Act
		// 	string profilePicURL = await this._profilePictureService.UpdateProfilePicture(profilePictureServiceModel);
        //
		// 	//Assert
		// 	Assert.IsNotEmpty(profilePicURL);
		// }
	}
}
