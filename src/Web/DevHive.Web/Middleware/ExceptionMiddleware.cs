using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DevHive.Web.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await this._next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			// Made to ressemble the formatting of property validation errors (like [MinLength(3)])
			string message = JsonConvert.SerializeObject(new {
								errors = new {
									Exception = new String[] { exception.Message }
								}
							});

			return context.Response.WriteAsync(message);
		}
	}
}
