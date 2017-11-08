using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace M183.UI.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString ActionMenuItem(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var tag = new TagBuilder("li");
            if (htmlHelper.ViewContext.RequestContext.IsCurrentRoute(null, controllerName, actionName))
            {
                tag.AddCssClass("selected");
            }
            tag.InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToString();
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}