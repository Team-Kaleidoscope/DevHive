using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class ErrorController
	{
		public HttpStatusCode Error(Exception exception)
		{
			return HttpStatusCode.OK;
		}
	}
}
