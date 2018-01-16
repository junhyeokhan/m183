using System;
using System.Linq;
using System.Web.Routing;

namespace M183.UI.Extensions
{
    /// <summary>
    /// 
    /// <see cref="http://geekswithblogs.net/bdiaz/archive/2010/04/09/handy-asp.net-mvc-2-extension-methods-ndash-where-am-i.aspx"/>
    /// </summary>
    public static class RequestExtensions
    {
        public static bool IsCurrentRoute(this RequestContext context, String areaName, String controllerName, params String[] actionNames)
        {
            var routeData = context.RouteData;
            var routeArea = routeData.DataTokens["area"] as String;
            var current = false;
            if (((String.IsNullOrEmpty(routeArea) && String.IsNullOrEmpty(areaName)) || (routeArea == areaName)) &&
                ((String.IsNullOrEmpty(controllerName)) || (routeData.GetRequiredString("controller") == controllerName)) &&
                ((actionNames == null) || actionNames.Contains(routeData.GetRequiredString("action"))))
            {
                current = true;
            }
            return current;
        }
    }
}