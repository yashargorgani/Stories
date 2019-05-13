using Stories.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace Stories.Service.Helpers
{
    public static class ExceptionLogger
    {
        public static void LogException(this Exception exception, HttpRequestContext context)
        {
            string message = $"Exception: {exception.Style()}{Environment.NewLine}";

            if (context.Principal.Identity.IsAuthenticated)
                message += $"User Name: {context.Principal.Identity.Name}{Environment.NewLine}";
            else
                message += $"User Name: Anonymous{Environment.NewLine}";

            message += $"URL: {context.Url.Request.RequestUri}{Environment.NewLine}";

            message.Log();
        }
    }
}