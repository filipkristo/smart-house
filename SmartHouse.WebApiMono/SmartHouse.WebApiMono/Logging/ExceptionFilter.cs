using SmartHouse.WebApiMono.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http.Filters;

namespace SmartHouse.WebApiMono
{
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext context)
		{
            if(context?.Exception != null)
            {
                MainClass.Log.Error("HttpException", context.Exception);
                context.Response = context.Request.CreateResponse(context.Response.StatusCode, new Error(context.Exception.Message, context.Exception.StackTrace));
            }			
		}
	}
}
