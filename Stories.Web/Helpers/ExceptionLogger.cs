using Stories.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Stories.Web.Helpers
{
    public static class ExceptionLogger
    {
        public static void LogException(this Exception exception, HttpContextBase context)
        {
            string message = $"Exception: {exception.Style()} {Environment.NewLine}";

            if (context.User.Identity.IsAuthenticated)
                message += $"User Name: {context.User.Identity.Name}{Environment.NewLine}";
            else
                message += $"User Name: Anonymous{Environment.NewLine}";

            message += $"URL: {context.Request.Url}{Environment.NewLine}";
            message += $"Host Address: {context.Request.UserHostAddress}{Environment.NewLine}";            

            message.Log();
        }


    }
}