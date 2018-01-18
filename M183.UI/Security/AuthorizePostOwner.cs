using M183.BusinessLogic;
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
            Repository repository = new Repository();

            RouteData routeData = httpContext.Request.RequestContext.RouteData;

            int postId = 0;

            if (httpContext.Request.Params.AllKeys.Any(k => k == "postId"))
            {
                postId = repository.GetPostId(httpContext.Request.Params.GetValues("postId")[0]);
            }
            else
            {
                postId = repository.GetPostId(httpContext.Request.Params.GetValues("Id")[0]);
            }

            return BusinessUser.Current.IsAuthorisedToPost(postId);
        }
    }
}