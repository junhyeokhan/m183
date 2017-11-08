using System;
using System.Linq;
using System.Web.Routing;

namespace M183.UI.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsCurrentRoute(this RequestContext context, string areaName, string controllerName, params string[] actionNames)
        {
            var routeData = context.RouteData;
            var routeArea = routeData.DataTokens["area"] as String;
            var current = false;
            if (((string.IsNullOrEmpty(routeArea) && string.IsNullOrEmpty(areaName)) || (routeArea == areaName)) &&
                 ((string.IsNullOrEmpty(controllerName)) || (routeData.GetRequiredString("controller") == controllerName)) &&
                 ((actionNames == null) || actionNames.Contains(routeData.GetRequiredString("action"))))
            {
                current = true;
            }
            return current;
        }
    }
}