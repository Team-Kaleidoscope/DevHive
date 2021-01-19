using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DevHive.Web.Models.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		// private readonly ILogger _logger;

		public ExceptionMiddleware(RequestDelegate next)
		{
			this._next = next;
			// this._logger = logger;
		}
		// public ExceptionMiddleware(RequestDelegate next, ILogger logger)
		// {
		// 	this._logger = logger;
		// 	this._next = next;
		// }

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await this._next(httpContext);
			}
			catch (Exception ex)
			{
				// this._logger.LogError($"Something went wrong: {ex}");
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			return context.Response.WriteAsync(new
			{
				StatusCode = context.Response.StatusCode,
				Message = exception.Message
			}.ToString());
		}
	}
}
