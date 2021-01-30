using System.Threading.Tasks;
using DevHive.Services.Models;

namespace DevHive.Services.Interfaces
{
	public interface IFeedService
	{
		Task<ReadPageServiceModel> GetPage(GetPageServiceModel getPageServiceModel);
		Task<ReadPageServiceModel> GetUserPage(GetPageServiceModel model);
	}
}
