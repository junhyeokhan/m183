using System.Web;
using System.Web.Mvc;
using M183.BusinessLogic.Models;

namespace M183.UI.Security
{
    public class AuthorizeUser : AuthorizeAttribute
    {
        // Reference: https://stackoverflow.com/questions/11493873/how-to-implement-custom-authorize-attribute-for-the-following-case
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return BusinessUser.Current.IsAuthenticated;
        }
    }
}