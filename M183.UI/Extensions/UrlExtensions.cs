using System.Web.Mvc;

namespace M183.UI.Extensions
{
    public static class UrlExtensions
    {
        public static bool IsCurrent(this UrlHelper urlHelper, string areaName, string controllerName, params string[] actionNames)
        {
            return urlHelper.RequestContext.IsCurrentRoute(areaName, controllerName, actionNames);
        }

        public static string Selected(this UrlHelper urlHelper, string areaName, string controllerName, params string[] actionNames)
        {
            return urlHelper.IsCurrent(areaName, controllerName, actionNames) ? "selected" : string.Empty;
        }
    }
}