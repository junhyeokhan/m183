using System;
using System.Web.Mvc;

namespace M183.UI.Extensions
{
    /// <summary>
    /// 
    /// <see cref="http://geekswithblogs.net/bdiaz/archive/2010/04/09/handy-asp.net-mvc-2-extension-methods-ndash-where-am-i.aspx"/>
    /// </summary>
    public static class UrlExtensions
    {
        public static bool IsCurrent(this UrlHelper urlHelper, String areaName, String controllerName, params String[] actionNames)
        {
            return urlHelper.RequestContext.IsCurrentRoute(areaName, controllerName, actionNames);
        }

        public static string Selected(this UrlHelper urlHelper, String areaName, String controllerName, params String[] actionNames)
        {
            return urlHelper.IsCurrent(areaName, controllerName, actionNames) ? "selected" : String.Empty;
        }
    }
}