using System;
using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DevHive.Web.Controllers
{
	public class ErrorController : ControllerBase
	{
		[HttpPost]
		[Route("/api/Error")]
		public IActionResult Error()
		{
			//Later for logging
			string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

			IExceptionHandlerFeature exception =
			HttpContext.Features.Get<IExceptionHandlerFeature>();
		
			object result = ProcessException(requestId, exception);
			return new BadRequestObjectResult(JsonConvert.SerializeObject(result));
		}

		private object ProcessException(string requestId, IExceptionHandlerFeature exception)
		{
			switch (exception.Error)
			{
				case ArgumentException _:
				case InvalidOperationException _:
				case AutoMapperMappingException _:
				case AutoMapperConfigurationException _:
					return MessageToObject(exception.Error.Message);
				default:
					return MessageToObject(null);
			}
		}

		private object MessageToObject(string message)
		{
			return new
			{
				Error = message
			};
		}
	}
}

