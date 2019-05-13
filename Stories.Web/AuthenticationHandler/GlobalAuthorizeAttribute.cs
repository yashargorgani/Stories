using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Stories.Web
{
    public class GlobalAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsGlobalAuthenticate();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var routeValues = new RouteValueDictionary(new
            {
                controller = "Account",
                action = "Login",
            });
            filterContext.Result = new RedirectToRouteResult(routeValues);
        }
    }
}