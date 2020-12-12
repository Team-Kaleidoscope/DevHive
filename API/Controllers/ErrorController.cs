using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class ErrorController
	{
		[HttpGet]
		public HttpStatusCode HttpError(HttpRequestException exception)
		{
			Console.WriteLine("WE HERE, BOIIIIIII");

			return HttpStatusCode.OK;
		}
	}
}
