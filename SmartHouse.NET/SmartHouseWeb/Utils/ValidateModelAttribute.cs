using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SmartHouseWeb.Utils
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {        
        public ValidateModelAttribute()
        {            
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false && actionContext.Request.Method != HttpMethod.Get)
            {                
                var errors = new List<ValidationMessage>();
                foreach (var state in actionContext.ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(new ValidationMessage() { Property = state.Key, Message = error.ErrorMessage });                        
                    }
                }

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }

        private class ValidationMessage
        {
            public string Message { get; internal set; }
            public string Property { get; internal set; }
        }
    }
}