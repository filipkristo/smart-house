using Microsoft.ApplicationInsights;
using SmartHouseWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace SmartHouseWeb.Utils
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is Exception exception)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new Error(exception.ToString()));

                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
        }


    }
}