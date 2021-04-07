using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevHive.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Services
{
	public class CloudinaryService : ICloudService
	{
		// Regex for getting the filename without (final) filename extension
		// So, from image.png, it will match image, and from doc.my.txt will match doc.my
		private static readonly Regex s_imageRegex = new(".*(?=\\.)");

		private readonly Cloudinary _cloudinary;

		public CloudinaryService(string cloudName, string apiKey, string apiSecret)
		{
			this._cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
		}

		public async Task<List<string>> UploadFilesToCloud(List<IFormFile> formFiles)
		{
			List<string> fileUrls = new();
			foreach (var formFile in formFiles)
			{
				string fileName = s_imageRegex.Match(formFile.FileName).ToString();

				using var ms = new MemoryStream();
				formFile.CopyTo(ms);
				byte[] formBytes = ms.ToArray();

				RawUploadParams rawUploadParams = new()
				{
					File = new FileDescription(fileName, new MemoryStream(formBytes)),
					PublicId = fileName,
					UseFilename = true
				};

				RawUploadResult rawUploadResult = await this._cloudinary.UploadAsync(rawUploadParams);
				fileUrls.Add(rawUploadResult.Url.AbsoluteUri);
			}

			return fileUrls;
		}

		public async Task<bool> RemoveFilesFromCloud(List<string> fileUrls)
		{
			// Workaround, this method isn't fully implemented yet
			await Task.Run(() => {});

			return true;
		}
	}
}
