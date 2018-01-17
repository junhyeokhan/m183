using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace M183.UI.Extensions
{
    /// <summary>
    /// 
    /// <see cref="http://geekswithblogs.net/bdiaz/archive/2010/04/09/handy-asp.net-mvc-2-extension-methods-ndash-where-am-i.aspx"/>
    /// </summary>
    public static class HtmlExtensions
    {
        public static MvcHtmlString ActionMenuItem(this HtmlHelper htmlHelper, String linkText, String actionName, String controllerName, String htmlClass, String area)
        {
            var tag = new TagBuilder("li");
            if (htmlHelper.ViewContext.RequestContext.IsCurrentRoute(null, controllerName, actionName))
            {
                tag.AddCssClass("selected");
            }
            tag.AddCssClass(htmlClass);
            tag.InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, new { area = area }, null).ToString();
            return MvcHtmlString.Create(tag.ToString());
        }

    }
}