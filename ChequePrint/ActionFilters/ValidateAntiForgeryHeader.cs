using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChequePrint.ActionFilters
{
    /// <summary>
    /// Action filter to make sure that the anti forgery token is included in the request
    /// </summary>
    public class ValidateAntiForgeryHeader : ActionFilterAttribute
    {
        private const string KEY_NAME = "__RequestVerificationToken";

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string formToken = "", cookieToken = "";

            // Try to retrieve the cookie / server token
            var cookie = actionContext.Request.Headers.GetCookies(KEY_NAME).FirstOrDefault();
            if (cookie != null)
                cookieToken = cookie[KEY_NAME].Value;

            // Try to retrieve the form / client token
            formToken = actionContext.Request.Headers.GetValues(KEY_NAME).FirstOrDefault();

            // Try validating the tokens
            System.Web.Helpers.AntiForgery.Validate(cookieToken, formToken);

            base.OnActionExecuting(actionContext);
        }
    }
}