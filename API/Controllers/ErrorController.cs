using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	public class ErrorController
	{
		[HttpGet]
		[Route("HttpError")]
		public HttpStatusCode HttpError(HttpRequestException exception)
		{
			Console.WriteLine("WE HERE, BOIIIIIII");

			return HttpStatusCode.OK;
		}
	}
}
