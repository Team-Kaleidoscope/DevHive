using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class ErrorController
	{
		[HttpGet]
		public IActionResult Error(Exception exception)
		{
			return new BadRequestObjectResult(exception);
		}
	}
}

