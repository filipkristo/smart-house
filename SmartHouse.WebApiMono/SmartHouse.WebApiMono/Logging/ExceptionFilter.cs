using System;
using System.Diagnostics;
using System.Web.Http.Filters;

namespace SmartHouse.WebApiMono
{
	public class ExceptionFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
            if(actionExecutedContext?.Exception != null)
            {
                MainClass.Log.Error("HttpException", actionExecutedContext.Exception);                
            }			
		}
	}
}
