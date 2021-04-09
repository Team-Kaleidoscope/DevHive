using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Services.Interfaces
{
	public interface ICloudService
	{
		Task<List<string>> UploadFilesToCloud(List<IFormFile> formFiles);

		Task<bool> RemoveFilesFromCloud(List<string> fileUrls);
	}
}
