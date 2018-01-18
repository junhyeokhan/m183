using M183.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace M183.UI.Security
{
    public class AuthorizePostOwner : AuthorizeAttribute
    {
        // Reference: https://stackoverflow.com/questions/11493873/how-to-implement-custom-authorize-attribute-for-the-following-case
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            RouteData routeData = httpContext.Request.RequestContext.RouteData;

            int postId = 0;

            if (httpContext.Request.Params.AllKeys.Any(k => k == "postId"))
            {
                int.TryParse(httpContext.Request.Params.GetValues("postId")[0], out postId);
            }
            else
            {
                int.TryParse(httpContext.Request.Params.GetValues("Id")[0], out postId);
            }

            return BusinessUser.Current.IsAuthorisedToPost(postId);
        }
    }
}