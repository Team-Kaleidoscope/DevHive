using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DevHive.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DevHive.Services.Services
{
	public class CloudinaryService : ICloudService
	{
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
				string formFileId = Guid.NewGuid().ToString();

				using (var ms = new MemoryStream())
				{
					formFile.CopyTo(ms);
					byte[] formBytes = ms.ToArray();

					RawUploadParams rawUploadParams = new()
					{
						File = new FileDescription(formFileId, new MemoryStream(formBytes)),
						PublicId = formFileId,
						UseFilename = true
					};

					RawUploadResult rawUploadResult = await this._cloudinary.UploadAsync(rawUploadParams);
					fileUrls.Add(rawUploadResult.Url.AbsoluteUri);
				}
			}

			return fileUrls;
		}

		public async Task<bool> RemoveFilesFromCloud(List<string> fileUrls)
		{
			return true;
		}
	}
}
