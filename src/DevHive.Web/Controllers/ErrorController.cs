using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class ErrorController
	{
		[HttpGet]
		public HttpStatusCode Error(HttpRequestException exception)
		{	
			return BadRequest(exception)
		}
	}
}
